using System;
using System.Collections.Generic;

using Assets._Achromatic.Scripts.Beatmap;
using Assets._Achromatic.Scripts.Pieces;

namespace Assets._Achromatic.Scripts.Scores {

    public class Criteria {

        public Criteria(BeatmapData beatmapData, ScoreTracker scoreTracker) {
            this.scoreTracker = scoreTracker;

            // pre-calculate all judge timings
            timings = new();

            float beat0time = beatmapData.PreludeLength;
            float secondPerBeat = 60.0f / beatmapData.Tempo;

            foreach (BeatmapNote note in beatmapData.notes) {
                // per note
                float centerTiming = secondPerBeat * note.CalcBeatCount()
                        + beat0time;

                // todo allow different actions for single note type
                PressedActions action = note.type switch {
                    BeatmapNoteType.JUMP => PressedActions.JUMP,
                    BeatmapNoteType.DASH => PressedActions.DASH,
                    _ => PressedActions.NONE
                };

                timings.Enqueue(new Timing(centerTiming, action));
            }
        }

        public Hit Judge(float time, PressedActions actions) {
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
        /// </summary>
        public void Update(float musicTime) {
            if (timings.Count <= 0) {
                return;
            }

            while (timings.Peek().IsPassByMiss(musicTime)) {
                timings.Dequeue();
                scoreTracker.Record(Hit.LATE_MISS);
                if (timings.Count <= 0) {
                    break;
                }
            }
        }

        /// <summary>
        /// pre-calculated all timings during creation
        /// </summary>
        private readonly Queue<Timing> timings;

        private readonly ScoreTracker scoreTracker;

    }


}