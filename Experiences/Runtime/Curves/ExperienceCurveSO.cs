using UnityEngine;

namespace CupkekGames.Resources.Experiences.Curves
{
    /// <summary>
    /// Abstract base for a level-cost curve. Each subclass authors the formula or
    /// data backing it (polynomial, linear, stepped lookup, animation-curve sampled, etc.)
    /// and is registered in an <see cref="ExperienceCurveCatalog"/> so that
    /// <see cref="ExperienceDefinitionSO"/> assets can reference it by <c>CatalogKey</c>.
    /// </summary>
    /// <remarks>
    /// One method to implement: <see cref="GetRequiredExperience"/>, returning the amount
    /// of XP required to traverse from <c>level</c> to <c>level + 1</c>. <see cref="ExperienceHelper"/>
    /// composes these per-level increments into total-XP queries.
    /// </remarks>
    public abstract class ExperienceCurveSO : ScriptableObject
    {
        /// <summary>XP cost to advance from the given level to the next.</summary>
        public abstract int GetRequiredExperience(int level);
    }
}
