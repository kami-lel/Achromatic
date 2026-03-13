// Criteria.cs
using System.Collections.Generic;
using Assets._Achromatic.Scripts.Beatmaps;
using Assets._Achromatic.Scripts.Pieces;
using Assets._Achromatic.Scripts.Players;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Scores {
    [RequireComponent(typeof(MusicManager))]
    [RequireComponent(typeof(Score))]
    public class Criteria: MonoBehaviour {

        // Public API  #########################################################

        public Hit Judge(Actions actions) {
            if (timings.Count == 0)
                return Hit.NO_HIT;

            var t = timings.Peek();

            // Do NOT dequeue unless player actually attempted a hit
            if (actions == Actions.NONE)
                return Hit.NO_HIT;

            if (!t.IsInJudgingRange(music.Time))
                return Hit.NO_HIT;

            timings.Dequeue();
            return t.Judge(music.Time, actions);
        }

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            music = GetComponent<MusicManager>();
            if (music == null) {
                Debug.LogError("fail to get: MusicManager");
            }
            score = GetComponent<Score>();
            if (score == null) {
                Debug.LogError("fail to get: Score");
            }
        }

        public void Update() {
            if (GCS.I.states != GameState.MAIN_PIECE || timings.Count == 0)
                return;

            // auto-miss notes you fully passed
            while (timings.Count > 0 && timings.Peek().IsPassByMiss(music.Time)) {
                timings.Dequeue();
                score.Record(Hit.LATE_MISS);
            }
        }

        // private members  ####################################################
        private readonly Queue<Timing> timings;

        // Cached References
        private MusicManager music;
        private Score score;

        // BUG  criteria fix
        // TODO need to work w/ half start


        /*

        public Criteria(PieceScript piece) {
            p = piece;
            timings = new Queue<Timing>();

            float spb = 60f / p.beatmap.meta.tempo; // seconds per beat

            // Build full timing list
            foreach (var note in p.beatmap.notesQ) {
                float beat = p.beatmap.CalcBeatCount(note);
                float center = p.beatmap.meta.preludeSeconds + beat * spb;

                // TODO detach note type from action type
                Actions action = note.type switch {
                    NoteType.JUMP => Actions.JUMP,
                    NoteType.DASH => Actions.SQUAT,
                    _ => Actions.NONE
                };

                timings.Enqueue(new Timing(center, action, p.beatmap.meta));
            }

            // IMPORTANT: seek to current playback time so next judged note
            // matches next rendered note.
            SeekToTime(p.music.Time);
        }
        */

        public void SeekToTime(float timeSeconds) {
            // Drop all notes that are already "too late to ever hit"
            // i.e., passed rightGoodBound.
            while (timings.Count > 0 && timings.Peek().IsPassByMiss(timeSeconds))
                timings.Dequeue();
        }
    }
}