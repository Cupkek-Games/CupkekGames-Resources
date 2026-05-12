using CupkekGames.Data;

namespace CupkekGames.Resources.Experiences
{
    public static class ExperienceConstants
    {
        /// <summary>Catalog id for <see cref="ExperienceDefinitionSO"/> assets; register an <see cref="AssetCatalog{T}"/> with this id.</summary>
        public const string ExperiencesCatalogId = "Experiences";

        /// <summary>Catalog id for <see cref="Curves.ExperienceCurveSO"/> assets; register an <see cref="AssetCatalog{T}"/> with this id.</summary>
        public const string ExperienceCurvesCatalogId = "ExperienceCurves";

        /// <summary>Default cap when no per-track maxLevel is set. Matches the value that lived in the migrated ExperienceHelper.</summary>
        public const int DefaultMaxLevel = 90;
    }
}
