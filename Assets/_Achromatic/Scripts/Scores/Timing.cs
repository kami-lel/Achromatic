
using Assets._Achromatic.Scripts.Pieces;

namespace Assets._Achromatic.Scripts.Scores {

    /// <summary>
    /// timing for a single note
    /// </summary>
    public class Timing {

        private readonly float startJudgeBound;
        private readonly float center;

        private readonly float leftGoodBound;
        private readonly float leftGreatBound;
        private readonly float leftPerfectBound;
        private readonly float rightGoodBound;
        private readonly float rightGreatBound;
        private readonly float rightPerfectBound;

        private readonly PressedActions allowedAction;

        public Timing(
                float centerTiming,
                PressedActions action,
                float perfectDelta,
                float greatDelta,
                float goodDelta
            ) {
            center = centerTiming;
            allowedAction = action;

            leftGoodBound = centerTiming - goodDelta;
            leftGreatBound = centerTiming - greatDelta;
            leftPerfectBound = centerTiming - perfectDelta;
            rightGoodBound = centerTiming + goodDelta;
            rightGreatBound = centerTiming + greatDelta;
            rightPerfectBound = centerTiming + perfectDelta;

            startJudgeBound = leftGoodBound - goodDelta;
        }

        public Hit Judge(float time, PressedActions action) {
            if (action != allowedAction) {
                return Hit.INCORRECT;
            }

            if (time < leftGoodBound) {
                return Hit.EARLY_MISS;
            } else if (time < leftGreatBound) {
                return Hit.EARLY_GOOD;
            } else if (time < leftPerfectBound) {
                return Hit.EARLY_GREAT;
            } else if (time < center) {
                return Hit.EARLY_PERFECT;
            } else if (time < rightPerfectBound) {
                return Hit.LATE_PERFECT;
            } else if (time < rightGreatBound) {
                return Hit.LATE_GREAT;
            } else if (time < rightGoodBound) {
                return Hit.LATE_GOOD;
            } else {
                return Hit.LATE_MISS;
            }
        }

        public bool IsPassByMiss(float time) {
            return time > rightGoodBound;
        }

        public bool IsInJudgingRange(float time) {
            return time > startJudgeBound;
        }
    }
}