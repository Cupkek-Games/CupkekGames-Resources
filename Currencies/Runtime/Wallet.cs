using System;
using System.Collections.Generic;
using CupkekGames.Data;
using UnityEngine;
using CurrencyPair = CupkekGames.KeyValueDatabases.KeyValuePair<string, long>;

namespace CupkekGames.Resources.Currencies
{
    /// <summary>
    /// Runtime balance store keyed by currency id. Holds <c>long</c> amounts so
    /// idle-game-scale totals don't overflow. Implements <see cref="IData"/> so
    /// it slots cleanly into save-data structures.
    /// </summary>
    /// <remarks>
    /// Storage is serializer-agnostic (see <see cref="CupkekGames.KeyValueDatabases.KeyValueDatabase{TKey,TValue}"/> for the
    /// canonical pattern): the serialized source of truth is a Unity-serializable pair list —
    /// Unity binds the <c>[SerializeField]</c> field, reflection serializers (Newtonsoft, …)
    /// bind the public <see cref="Balances"/> property — and the runtime dictionary is a lazy,
    /// never-serialized cache.
    /// <para>
    /// <see cref="OnChanged"/> fires on <see cref="Set"/>, <see cref="Add"/>, and
    /// successful <see cref="Spend"/> with <c>(id, oldValue, newValue)</c>. Subscribe
    /// from UI binders that animate count-up tweens.
    /// </para>
    /// </remarks>
    [Serializable]
    public class Wallet : IData
    {
        /// <summary>Fires with <c>(currencyId, oldValue, newValue)</c> on every successful mutation.</summary>
        public event Action<string, long, long> OnChanged;

        [SerializeField] private List<CurrencyPair> _balances = new();

        [NonSerialized] private Dictionary<string, long> _cache;

        public Wallet() { }

        public Wallet(Wallet other)
        {
            if (other?._balances == null)
                return;
            foreach (CurrencyPair pair in other._balances)
                _balances.Add(new CurrencyPair { Key = pair.Key, Value = pair.Value });
        }

        /// <summary>
        /// Serialized pairs, exposed for reflection-based serializers. Unity ignores
        /// properties and binds the backing field directly.
        /// </summary>
        public List<CurrencyPair> Balances
        {
            get => _balances;
            set
            {
                _balances = value ?? new List<CurrencyPair>();
                _cache = null;
            }
        }

        private Dictionary<string, long> Cache
        {
            get
            {
                if (_cache == null)
                {
                    _cache = new Dictionary<string, long>(_balances.Count);
                    foreach (CurrencyPair pair in _balances)
                        _cache.TryAdd(pair.Key, pair.Value);
                }
                return _cache;
            }
        }

        private void SetRaw(string id, long amount)
        {
            if (Cache.ContainsKey(id))
            {
                Cache[id] = amount;
                int index = _balances.FindIndex(pair => pair.Key == id);
                if (index >= 0)
                    _balances[index] = new CurrencyPair { Key = id, Value = amount };
            }
            else
            {
                Cache.Add(id, amount);
                _balances.Add(new CurrencyPair { Key = id, Value = amount });
            }
        }

        public long Get(string id) =>
            !string.IsNullOrEmpty(id) && Cache.TryGetValue(id, out long v) ? v : 0L;

        public bool Has(string id) =>
            !string.IsNullOrEmpty(id) && Cache.ContainsKey(id);

        public bool CanAfford(string id, long amount) => Get(id) >= amount;

        /// <summary>Replaces the stored amount and fires <see cref="OnChanged"/> if it differs from the prior value.</summary>
        public void Set(string id, long amount)
        {
            if (string.IsNullOrEmpty(id))
                return;
            long old = Get(id);
            if (old == amount)
                return;
            SetRaw(id, amount);
            OnChanged?.Invoke(id, old, amount);
        }

        /// <summary>Increments by <paramref name="amount"/>. Negative amounts subtract but do not clamp below zero — use <see cref="Spend"/> for safe deductions.</summary>
        public void Add(string id, long amount)
        {
            if (string.IsNullOrEmpty(id) || amount == 0L)
                return;
            long old = Get(id);
            long next = old + amount;
            SetRaw(id, next);
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
            SetRaw(id, next);
            OnChanged?.Invoke(id, old, next);
            return true;
        }

        public IData CloneData() => new Wallet(this);

        public bool Validate() => true;

        public void OnAfterDeserialize()
        {
            _cache = null;
        }
    }
}
