using System;
using System.Collections.Generic;

using Assets._Achromatic.Scripts.Beatmap;

namespace Assets._Achromatic.Scripts.Scores {

    public class Criteria {

        public EventHandler onDetectPassByMiss;

        /// <summary>
        /// pre-calculated all timings during creation
        /// </summary>
        private Queue<Timing> timings;

        public Criteria(BeatmapData beatmap) {
            // pre-calculate all judge timings
            timings = new();

            float beat0time = beatmap.PreludeLength;
            float secondPerBeat = 60.0f / beatmap.Tempo;

            foreach (BeatmapNote note in beatmap.notes) {
                // per note
                float centerTiming = secondPerBeat * note.CalcBeatCount()
                        + beat0time;

                // todo allow different actions for single note type
                InputPressedActions action = note.type switch {
                    BeatmapNoteType.JUMP => InputPressedActions.JUMP,
                    BeatmapNoteType.DASH => InputPressedActions.DASH,
                    _ => InputPressedActions.NONE
                };

                timings.Enqueue(new Timing(centerTiming, action));
            }
        }

        public Hit Judge(float time, InputPressedActions actions) {
            if (timings.Count <= 0) {
                return Hit.NO_HIT;
            }

            Timing timing = timings.Peek();
            if (timing.IsInJudgingRange(time)) {
                timing = timings.Dequeue();
                return timing.Judge(time, actions);

            } else {
                return Hit.NO_HIT;
            }
        }

        /// <summary>
        /// detect miss then player is too far away
        ///
        /// used in <c>Update()</c>
        /// </summary>
        public void DetectPassByMiss(float time) {
            if (timings.Count <= 0) {
                return;
            }

            while (timings.Peek().IsPassByMiss(time)) {
                // detect pass-by miss
                onDetectPassByMiss?.Invoke(this, EventArgs.Empty);

                timings.Dequeue();
                if (timings.Count <= 0) {
                    break;
                }
            }
        }

        public class Timing {

            // todo dynamic time deltas
            private const float perfectDelta = 0.05f;
            private const float greatDelta = 0.10f;
            private const float goodDelta = 0.30f;

            private readonly float startJudgeBound;
            private readonly float center;

            private readonly float leftGoodBound;
            private readonly float leftGreatBound;
            private readonly float leftPerfectBound;
            private readonly float rightGoodBound;
            private readonly float rightGreatBound;
            private readonly float rightPerfectBound;

            private readonly InputPressedActions allowedAction;

            public Timing(float centerTiming, InputPressedActions action) {
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

            public Hit Judge(float time, InputPressedActions action) {
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
}