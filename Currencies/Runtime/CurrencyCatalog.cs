using CupkekGames.Data;
using UnityEngine;

namespace CupkekGames.Resources.Currencies
{
    /// <summary>
    /// Catalog of <see cref="CurrencyDefinitionSO"/> assets. Set the catalog's
    /// <c>_catalogId</c> to <see cref="CurrencyConstants.CurrenciesCatalogId"/>
    /// (the conventional value <c>"Currencies"</c>) so consumers can resolve it
    /// via <c>ServiceLocator.Get&lt;IAssetCatalog&lt;CurrencyDefinitionSO&gt;&gt;("Currencies")</c>.
    /// </summary>
    [CreateAssetMenu(
        fileName = "CurrencyCatalog",
        menuName = "CupkekGames/Resources/Catalog/Currencies")]
    public class CurrencyCatalog : AssetCatalog<CurrencyDefinitionSO> { }
}
