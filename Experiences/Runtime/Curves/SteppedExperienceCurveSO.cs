using UnityEngine;

namespace CupkekGames.Resources.Experiences.Curves
{
    /// <summary>
    /// XP required is read directly from a designer-authored array. <c>levelCosts[0]</c>
    /// is the XP cost to advance from level 0 → 1, <c>levelCosts[1]</c> from 1 → 2, etc.
    /// Levels beyond the array's length fall back to the last entry.
    /// </summary>
    /// <remarks>
    /// Use when each level needs hand-tuned values that don't fit a closed-form
    /// formula — early-game ramp, mid-game plateau, late-game spike, etc.
    /// </remarks>
    [CreateAssetMenu(
        fileName = "SteppedExperienceCurve",
        menuName = "CupkekGames/Resources/Experience Curve/Stepped")]
    public class SteppedExperienceCurveSO : ExperienceCurveSO
    {
        [Tooltip("Per-level XP costs. Index N = cost to go from level N to N+1.")]
        [SerializeField] private int[] _levelCosts = new int[] { 10, 30, 60, 100 };

        public override int GetRequiredExperience(int level)
        {
            if (_levelCosts == null || _levelCosts.Length == 0)
                return 1;
            int idx = level < 0 ? 0 : level;
            if (idx >= _levelCosts.Length)
                idx = _levelCosts.Length - 1;
            return Mathf.Max(1, _levelCosts[idx]);
        }
    }
}
