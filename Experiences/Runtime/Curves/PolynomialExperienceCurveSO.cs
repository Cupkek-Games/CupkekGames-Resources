using UnityEngine;

namespace CupkekGames.Resources.Experiences.Curves
{
    /// <summary>
    /// XP required = <c>round(offset + multiplier * level^power)</c>.
    /// HeroManager's legacy <c>10 + 10 * level²</c> formula fits as
    /// <c>offset = 10, multiplier = 10, power = 2</c>.
    /// </summary>
    [CreateAssetMenu(
        fileName = "PolynomialExperienceCurve",
        menuName = "CupkekGames/Resources/Experience Curve/Polynomial")]
    public class PolynomialExperienceCurveSO : ExperienceCurveSO
    {
        [Tooltip("Constant added before the polynomial term.")]
        [SerializeField] private float _offset = 10f;

        [Tooltip("Coefficient on the polynomial term.")]
        [SerializeField] private float _multiplier = 10f;

        [Tooltip("Exponent applied to the level value.")]
        [SerializeField] private float _power = 2f;

        public override int GetRequiredExperience(int level)
        {
            if (level <= 0)
                return Mathf.Max(1, Mathf.RoundToInt(_offset));
            float raw = _offset + _multiplier * Mathf.Pow(level, _power);
            return Mathf.Max(1, Mathf.RoundToInt(raw));
        }
    }
}
