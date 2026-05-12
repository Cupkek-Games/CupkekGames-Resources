using System;

namespace CupkekGames.Resources.Currencies
{
    /// <summary>
    /// An (id, amount) pair identifying a currency balance. Used as the value type
    /// for drop results, transactions, and serialization where a transient pair
    /// is more useful than mutating a <see cref="Wallet"/>.
    /// </summary>
    [Serializable]
    public struct Currency
    {
        public string Id;
        public long Amount;

        public Currency(string id, long amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}
