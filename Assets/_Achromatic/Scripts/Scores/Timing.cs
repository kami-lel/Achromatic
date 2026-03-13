
using System;
using Assets._Achromatic.Scripts.Beatmaps;
using Assets._Achromatic.Scripts.Players;

namespace Assets._Achromatic.Scripts.Scores {

    /// <summary>
    /// timing for a single note
    /// </summary>
    public class Timing {

        // Public API  #########################################################
        public Hit Judge(float time, Actions action) {
            if ((action & allowedAction) == 0) {
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

        public bool IsMissedByPassing(float time) {
            return time > rightGoodBound;
        }

        public bool IsInJudgingRange(float time) {
            return time > startJudgeBound;
        }

        // Constructor  ########################################################
        public Timing(Beatmap beatmap, Note note) {
            // calc center timing
            float beat = beatmap.CalcBeatCount(note);
            float centerTiming = beatmap.meta.preludeSeconds +
                    beat * beatmap.secondsPerBeat;

            leftGoodBound = centerTiming - beatmap.meta.goodDeltaSecond;
            leftGreatBound = centerTiming - beatmap.meta.greatDeltaSecond;
            leftPerfectBound = centerTiming - beatmap.meta.perfectDeltaSecond;
            rightGoodBound = centerTiming + beatmap.meta.goodDeltaSecond;
            rightGreatBound = centerTiming + beatmap.meta.greatDeltaSecond;
            rightPerfectBound = centerTiming + beatmap.meta.perfectDeltaSecond;

            startJudgeBound = leftGoodBound - beatmap.meta.goodDeltaSecond;

            // decide allowedAction  -------------------------------------------
            allowedAction = note.type switch {
                "jump" => Actions.JUMP,
                "squat" => Actions.SQUAT,
                "attack" => Actions.ATTACK,
                _ => throw new ArgumentException($"unknown note type: {note.type}")
            };
        }


        // private members  ####################################################
        private readonly float startJudgeBound;
        private readonly float center;

        private readonly float leftGoodBound;
        private readonly float leftGreatBound;
        private readonly float leftPerfectBound;
        private readonly float rightGoodBound;
        private readonly float rightGreatBound;
        private readonly float rightPerfectBound;

        private readonly Actions allowedAction;
    }
}