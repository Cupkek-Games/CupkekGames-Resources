using System;
using System.Collections.Generic;
using CupkekGames.Data;

namespace CupkekGames.Resources.Currencies
{
    /// <summary>
    /// Runtime balance store keyed by currency id. Holds <c>long</c> amounts so
    /// idle-game-scale totals don't overflow. Implements <see cref="IData"/> so
    /// it slots cleanly into save-data structures.
    /// </summary>
    /// <remarks>
    /// <see cref="OnChanged"/> fires on <see cref="Set"/>, <see cref="Add"/>, and
    /// successful <see cref="Spend"/> with <c>(id, oldValue, newValue)</c>. Subscribe
    /// from UI binders that animate count-up tweens.
    /// </remarks>
    [Serializable]
    public class Wallet : IData
    {
        /// <summary>Fires with <c>(currencyId, oldValue, newValue)</c> on every successful mutation.</summary>
        public event Action<string, long, long> OnChanged;

        private Dictionary<string, long> _balances = new();

        public Wallet() { }

        public Wallet(Wallet other)
        {
            if (other?._balances == null)
                return;
            foreach (KeyValuePair<string, long> pair in other._balances)
                _balances[pair.Key] = pair.Value;
        }

        public IReadOnlyDictionary<string, long> Balances => _balances;

        public long Get(string id) =>
            !string.IsNullOrEmpty(id) && _balances.TryGetValue(id, out long v) ? v : 0L;

        public bool Has(string id) =>
            !string.IsNullOrEmpty(id) && _balances.ContainsKey(id);

        public bool CanAfford(string id, long amount) => Get(id) >= amount;

        /// <summary>Replaces the stored amount and fires <see cref="OnChanged"/> if it differs from the prior value.</summary>
        public void Set(string id, long amount)
        {
            if (string.IsNullOrEmpty(id))
                return;
            long old = Get(id);
            if (old == amount)
                return;
            _balances[id] = amount;
            OnChanged?.Invoke(id, old, amount);
        }

        /// <summary>Increments by <paramref name="amount"/>. Negative amounts subtract but do not clamp below zero — use <see cref="Spend"/> for safe deductions.</summary>
        public void Add(string id, long amount)
        {
            if (string.IsNullOrEmpty(id) || amount == 0L)
                return;
            long old = Get(id);
            long next = old + amount;
            _balances[id] = next;
            OnChanged?.Invoke(id, old, next);
        }

        /// <summary>
        /// Deducts <paramref name="amount"/> if the balance can cover it. Returns <c>true</c>
        /// on success, <c>false</c> if insufficient. Does not mutate on failure.
        /// </summary>
        public bool Spend(string id, long amount)
        {
            if (string.IsNullOrEmpty(id) || amount <= 0L)
                return false;
            long old = Get(id);
            if (old < amount)
                return false;
            long next = old - amount;
            _balances[id] = next;
            OnChanged?.Invoke(id, old, next);
            return true;
        }

        public IData CloneData() => new Wallet(this);

        public bool Validate() => true;

        public void OnAfterDeserialize() { }
    }
}
