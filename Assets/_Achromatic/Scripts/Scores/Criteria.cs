using System;
using System.Collections.Generic;

using Assets._Achromatic.Scripts.Beatmap;
using Assets._Achromatic.Scripts.Pieces;

namespace Assets._Achromatic.Scripts.Scores {

    public class Criteria {

        public Criteria(BeatmapData beatmap, ScoreTracker scoreTracker, Music music) {
            this.scoreTracker = scoreTracker;
            this.music = music;

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

        public void Update(Phase phase) {
            if (phase == Phase.MAIN_PLAY) {
                DetectPassByMiss();
            }
        }

        /// <summary>
        /// detect miss then player is too far away
        ///
        /// used in <c>Update()</c>
        /// </summary>
        private void DetectPassByMiss() {
            if (timings.Count <= 0) {
                return;
            }

            while (timings.Peek().IsPassByMiss(music.Time)) {
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

        private ScoreTracker scoreTracker;
        private Music music;


    }


}