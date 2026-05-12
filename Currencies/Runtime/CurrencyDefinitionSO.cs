using UnityEngine;

namespace CupkekGames.Resources.Currencies
{
    /// <summary>
    /// Defines a single currency (e.g. Gold, Diamond, Energy). Create one SO asset per currency.
    /// Registered in a <see cref="CurrencyCatalog"/> and looked up by the SO's asset name.
    /// </summary>
    [CreateAssetMenu(
        fileName = "CurrencyDefinition",
        menuName = "CupkekGames/Resources/Currency Definition")]
    public class CurrencyDefinitionSO : ScriptableObject
    {
        [Tooltip("Display name shown in UI (e.g. 'Gold', 'Diamond').")]
        [SerializeField] private string _displayName;

        [Tooltip("Icon shown in UI.")]
        [SerializeField] private Sprite _icon;

        [Tooltip("Maximum amount a Wallet may hold for this currency. 0 = unlimited.")]
        [SerializeField] private long _maxAmount;

        [Header("Display Format")]
        [Tooltip(".NET format string (e.g. 'N0' for integer with comma thousands separators).")]
        [SerializeField] private string _formatString = "N0";

        public string DisplayName => _displayName;
        public Sprite Icon => _icon;
        public long MaxAmount => _maxAmount;
        public string FormatString => _formatString;

        /// <summary>Format an amount for display using the configured format string.</summary>
        public string Beautify(long amount) => amount.ToString(_formatString);
    }
}
