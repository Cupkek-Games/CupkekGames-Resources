using CupkekGames.Data;
using UnityEngine;

namespace CupkekGames.Resources.Experiences.Curves
{
    /// <summary>
    /// Catalog of <see cref="ExperienceCurveSO"/> assets. Set the catalog's <c>_catalogId</c>
    /// to <see cref="ExperienceConstants.ExperienceCurvesCatalogId"/> (the conventional
    /// value <c>"ExperienceCurves"</c>). Multiple <see cref="ExperienceDefinitionSO"/>
    /// assets can reference the same curve via <c>CatalogKey</c> — change the curve
    /// once, every dependent track updates.
    /// </summary>
    [CreateAssetMenu(
        fileName = "ExperienceCurveCatalog",
        menuName = "CupkekGames/Resources/Catalog/Experience Curves")]
    public class ExperienceCurveCatalog : AssetCatalog<ExperienceCurveSO> { }
}
