// Criteria.cs
using System.Collections.Generic;
using Assets._Achromatic.Scripts.Beatmap;
using Assets._Achromatic.Scripts.Pieces;

namespace Assets._Achromatic.Scripts.Scores {
    public class Criteria {
        private readonly Queue<Timing> timings;
        private readonly PieceScript p;

        public Criteria(PieceScript piece) {
            p = piece;
            timings = new Queue<Timing>();

            float spb = 60f / p.beatmap.meta.tempo; // seconds per beat

            // Build full timing list
            foreach (var note in p.beatmap.notesQ) {
                float beat = p.beatmap.CalcBeatCount(note);
                float center = p.beatmap.meta.preludeSeconds + beat * spb;

                PressedActions action = note.type switch {
                    BeatmapNoteType.JUMP => PressedActions.JUMP,
                    BeatmapNoteType.DASH => PressedActions.DASH,
                    _ => PressedActions.NONE
                };

                timings.Enqueue(new Timing(center, action, p.beatmap.meta));
            }

            // IMPORTANT: seek to current playback time so next judged note
            // matches next rendered note.
            SeekToTime(p.music.Time);
        }

        public void SeekToTime(float timeSeconds) {
            // Drop all notes that are already "too late to ever hit"
            // i.e., passed rightGoodBound.
            while (timings.Count > 0 && timings.Peek().IsPassByMiss(timeSeconds))
                timings.Dequeue();
        }

        public Hit Judge(PressedActions actions) {
            if (timings.Count == 0)
                return Hit.NO_HIT;

            var t = timings.Peek();

            // Do NOT dequeue unless player actually attempted a hit
            if (actions == PressedActions.NONE)
                return Hit.NO_HIT;

            if (!t.IsInJudgingRange(p.music.Time))
                return Hit.NO_HIT;

            timings.Dequeue();
            return t.Judge(p.music.Time, actions);
        }

        public void Update() {
            if (GCS.I.states != GameState.MAIN_PIECE || timings.Count == 0)
                return;

            // auto-miss notes you fully passed
            while (timings.Count > 0 && timings.Peek().IsPassByMiss(p.music.Time)) {
                timings.Dequeue();
                p.scoreTracker.Record(Hit.LATE_MISS);
            }
        }
    }
}