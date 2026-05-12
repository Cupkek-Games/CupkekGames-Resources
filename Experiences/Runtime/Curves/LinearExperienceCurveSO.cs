using UnityEngine;

namespace CupkekGames.Resources.Experiences.Curves
{
    /// <summary>XP required = <c>round(offset + multiplier * level)</c>.</summary>
    [CreateAssetMenu(
        fileName = "LinearExperienceCurve",
        menuName = "CupkekGames/Resources/Experience Curve/Linear")]
    public class LinearExperienceCurveSO : ExperienceCurveSO
    {
        [Tooltip("Constant added before the linear term.")]
        [SerializeField] private float _offset = 10f;

        [Tooltip("XP added per level.")]
        [SerializeField] private float _multiplier = 10f;

        public override int GetRequiredExperience(int level)
        {
            if (level <= 0)
                return Mathf.Max(1, Mathf.RoundToInt(_offset));
            return Mathf.Max(1, Mathf.RoundToInt(_offset + _multiplier * level));
        }
    }
}
