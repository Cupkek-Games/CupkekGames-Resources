using System;

namespace CupkekGames.Resources.Experiences
{
    /// <summary>
    /// An (id, amount) pair identifying an experience-track delta. Used as the value
    /// type for drop results, event payloads, and serialization where a transient pair
    /// is more useful than mutating an <see cref="ExperienceTracker"/>.
    /// </summary>
    [Serializable]
    public struct Experience
    {
        public string Id;
        public long Amount;

        public Experience(string id, long amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}
