
using System;
using Assets._Achromatic.Scripts.Beatmaps;
using Assets._Achromatic.Scripts.Players;

namespace Assets._Achromatic.Scripts.Scores {

    /// <summary>
    /// timing for a single note
    /// </summary>
    public class Timing {

        // Public API  #########################################################
        public (Hit, int) Judge(float time, Actions action) {
            Hit hit;

            if ((action & allowedAction) == 0) {
                hit = Hit.INCORRECT;
            } else if (time < leftGoodBound) {
                hit = Hit.EARLY_MISS;
            } else if (time < leftGreatBound) {
                hit = Hit.EARLY_GOOD;
            } else if (time < leftPerfectBound) {
                hit = Hit.EARLY_GREAT;
            } else if (time < center) {
                hit = Hit.EARLY_PERFECT;
            } else if (time < rightPerfectBound) {
                hit = Hit.LATE_PERFECT;
            } else if (time < rightGreatBound) {
                hit = Hit.LATE_GREAT;
            } else if (time < rightGoodBound) {
                hit = Hit.LATE_GOOD;
            } else {
                hit = Hit.LATE_MISS;
            }

            return (hit, noteIdx);
        }

        public bool IsMissedByPassing(float time) {
            return time > rightGoodBound;
        }

        public bool IsInJudgingRange(float time) {
            return time > startJudgeBound;
        }

        // Constructor  ########################################################
        public Timing(Beatmap beatmap, Note note, int noteIdx) {
            // calc center timing
            float beat = beatmap.CalcNoteBeat(note);
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

            this.noteIdx = noteIdx;
        }


        // private members  ####################################################
        private int noteIdx;

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