using CupkekGames.Data;
using UnityEngine;

namespace CupkekGames.Resources.Experiences
{
    /// <summary>
    /// Catalog of <see cref="ExperienceDefinitionSO"/> assets. Set the catalog's
    /// <c>_catalogId</c> to <see cref="ExperienceConstants.ExperiencesCatalogId"/>
    /// (the conventional value <c>"Experiences"</c>).
    /// </summary>
    [CreateAssetMenu(
        fileName = "ExperienceCatalog",
        menuName = "CupkekGames/Resources/Catalog/Experiences")]
    public class ExperienceCatalog : AssetCatalog<ExperienceDefinitionSO> { }
}
