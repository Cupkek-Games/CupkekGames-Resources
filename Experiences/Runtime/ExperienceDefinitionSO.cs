using CupkekGames.Data;
using CupkekGames.Resources.Experiences.Curves;
using UnityEngine;

namespace CupkekGames.Resources.Experiences
{
    /// <summary>
    /// Defines a single experience track (e.g. "Hero", "Crafting", "Reputation").
    /// Carries display metadata and references an <see cref="ExperienceCurveSO"/> by
    /// <c>CatalogKey</c> so the curve math is authored once and reused across tracks.
    /// </summary>
    /// <remarks>
    /// The SO's asset name doubles as its id when consumers look it up via
    /// <see cref="ExperienceTracker"/> calls like <c>GetLevel("Hero")</c>. Match the
    /// asset name to the track's stable string id.
    /// </remarks>
    [CreateAssetMenu(
        fileName = "ExperienceDefinition",
        menuName = "CupkekGames/Resources/Experience Definition")]
    public class ExperienceDefinitionSO : ScriptableObject
    {
        [Tooltip("Display name shown in UI (e.g. 'Hero Level', 'Crafting Mastery').")]
        [SerializeField] private string _displayName;

        [Tooltip("Icon shown in UI.")]
        [SerializeField] private Sprite _icon;

        [Tooltip("Maximum level this track caps at. 0 = use ExperienceConstants.DefaultMaxLevel.")]
        [SerializeField] private int _maxLevel;

        [Tooltip("Curve registered in an ExperienceCurveCatalog. Defines XP-per-level cost.")]
        [CatalogKeyConstraint(ExperienceConstants.ExperienceCurvesCatalogId, typeof(ExperienceCurveSO))]
        [SerializeField] private CatalogKey _curveKey;

        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
        public int MaxLevel => _maxLevel > 0 ? _maxLevel : ExperienceConstants.DefaultMaxLevel;
        public CatalogKey CurveKey => _curveKey;
    }
}
