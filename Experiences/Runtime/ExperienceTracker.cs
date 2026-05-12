using System;
using System.Collections.Generic;
using CupkekGames.Data;
using CupkekGames.Resources.Experiences.Curves;
using CupkekGames.Services;

namespace CupkekGames.Resources.Experiences
{
    /// <summary>
    /// Runtime XP store keyed by track id. Holds <c>long</c> totals so idle-game-scale
    /// accumulations don't overflow. Implements <see cref="IData"/> so it slots cleanly
    /// into save-data structures.
    /// </summary>
    /// <remarks>
    /// Level computation is curve-driven. The tracker resolves each track's curve via
    /// <see cref="ServiceLocator"/> using the <see cref="ExperienceDefinitionSO.CurveKey"/>
    /// — register an <see cref="ExperienceCatalog"/> + <see cref="ExperienceCurveCatalog"/>
    /// before calling <see cref="GetLevel"/> or <see cref="AddExp"/> on a tracker.
    /// <para>
    /// <see cref="OnExperienceChanged"/> fires after every successful mutation.
    /// <see cref="OnLevelUp"/> fires when an <see cref="AddExp"/> call crosses one or
    /// more level thresholds — payload is <c>(id, oldLevel, newLevel)</c>.
    /// </para>
    /// </remarks>
    [Serializable]
    public class ExperienceTracker : IData
    {
        /// <summary>Fires with <c>(trackId, oldTotal, newTotal)</c> on every successful mutation.</summary>
        public event Action<string, long, long> OnExperienceChanged;

        /// <summary>Fires with <c>(trackId, oldLevel, newLevel)</c> when a mutation crosses a level threshold.</summary>
        public event Action<string, int, int> OnLevelUp;

        private Dictionary<string, long> _totals = new();

        public ExperienceTracker() { }

        public ExperienceTracker(ExperienceTracker other)
        {
            if (other?._totals == null)
                return;
            foreach (KeyValuePair<string, long> pair in other._totals)
                _totals[pair.Key] = pair.Value;
        }

        public IReadOnlyDictionary<string, long> Totals => _totals;

        public long Get(string id) =>
            !string.IsNullOrEmpty(id) && _totals.TryGetValue(id, out long v) ? v : 0L;

        public bool Has(string id) =>
            !string.IsNullOrEmpty(id) && _totals.ContainsKey(id);

        /// <summary>Replaces the stored total and fires <see cref="OnExperienceChanged"/> + <see cref="OnLevelUp"/> if the level crossed.</summary>
        public void Set(string id, long amount)
        {
            if (string.IsNullOrEmpty(id))
                return;
            long old = Get(id);
            if (old == amount)
                return;
            ExperienceCurveSO curve = ResolveCurve(id);
            int oldLevel = curve != null ? ExperienceHelper.GetLevel((int)old, curve.GetRequiredExperience) : 0;
            _totals[id] = amount;
            OnExperienceChanged?.Invoke(id, old, amount);
            if (curve != null)
            {
                int newLevel = ExperienceHelper.GetLevel((int)amount, curve.GetRequiredExperience);
                if (newLevel != oldLevel)
                    OnLevelUp?.Invoke(id, oldLevel, newLevel);
            }
        }

        /// <summary>Adds <paramref name="amount"/> to the track's total. Fires events as appropriate.</summary>
        public void AddExp(string id, long amount)
        {
            if (string.IsNullOrEmpty(id) || amount == 0L)
                return;
            long old = Get(id);
            long next = old + amount;
            ExperienceCurveSO curve = ResolveCurve(id);
            int oldLevel = curve != null ? ExperienceHelper.GetLevel((int)old, curve.GetRequiredExperience) : 0;
            _totals[id] = next;
            OnExperienceChanged?.Invoke(id, old, next);
            if (curve != null)
            {
                int newLevel = ExperienceHelper.GetLevel((int)next, curve.GetRequiredExperience);
                if (newLevel != oldLevel)
                    OnLevelUp?.Invoke(id, oldLevel, newLevel);
            }
        }

        /// <summary>Current level for the track. Returns 0 if no curve is resolvable.</summary>
        public int GetLevel(string id)
        {
            ExperienceCurveSO curve = ResolveCurve(id);
            return curve != null ? ExperienceHelper.GetLevel((int)Get(id), curve.GetRequiredExperience) : 0;
        }

        /// <summary>XP accumulated within the current level (0 to required-for-next-level).</summary>
        public int GetCurrentLevelExperience(string id)
        {
            ExperienceCurveSO curve = ResolveCurve(id);
            return curve != null ? ExperienceHelper.GetCurrentExperience((int)Get(id), curve.GetRequiredExperience) : 0;
        }

        /// <summary>XP required to traverse the current level.</summary>
        public int GetCurrentRequiredExperience(string id)
        {
            ExperienceCurveSO curve = ResolveCurve(id);
            if (curve == null)
                return 0;
            int level = ExperienceHelper.GetLevel((int)Get(id), curve.GetRequiredExperience);
            return ExperienceHelper.GetCurrentRequiredExperience(level, curve.GetRequiredExperience);
        }

        /// <summary>Fractional progress through the current level (0..1).</summary>
        public float GetExpPercent(string id)
        {
            ExperienceCurveSO curve = ResolveCurve(id);
            return curve != null ? ExperienceHelper.GetExpPercent((int)Get(id), curve.GetRequiredExperience) : 0f;
        }

        public IData CloneData() => new ExperienceTracker(this);

        public bool Validate() => true;

        public void OnAfterDeserialize() { }

        /// <summary>Resolves the curve for a track id via the registered ExperienceCatalog + ExperienceCurveCatalog. Returns null if either catalog isn't registered.</summary>
        private static ExperienceCurveSO ResolveCurve(string trackId)
        {
            if (string.IsNullOrEmpty(trackId))
                return null;
            IAssetCatalog<ExperienceDefinitionSO> defs =
                ServiceLocator.Get<IAssetCatalog<ExperienceDefinitionSO>>(
                    ExperienceConstants.ExperiencesCatalogId, silent: true);
            ExperienceDefinitionSO def = defs?.GetValue(trackId);
            if (def == null || def.CurveKey.IsEmpty)
                return null;
            IAssetCatalog<ExperienceCurveSO> curves =
                ServiceLocator.Get<IAssetCatalog<ExperienceCurveSO>>(def.CurveKey.Catalog, silent: true);
            return curves?.GetValue(def.CurveKey.Key);
        }
    }
}
