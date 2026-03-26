using System.Collections.Generic;
using Assets._Achromatic.Scripts.Beatmaps;
using Assets._Achromatic.Scripts.Pieces;
using Assets._Achromatic.Scripts.Players;
using UnityEngine;


namespace Assets._Achromatic.Scripts.Scores {

    [DefaultExecutionOrder(0)]
    [RequireComponent(typeof(MusicManager))]
    [RequireComponent(typeof(Score))]
    [RequireComponent(typeof(Beatmap))]
    public class Criteria: MonoBehaviour {

        // Public API  #########################################################

        public (Hit, int) Judge(Actions actions) {
            if (timings.Count == 0)
                return (Hit.NO_HIT, -1);

            Timing t = timings.Peek();

            // Do NOT dequeue unless player actually attempted a hit
            if (actions == Actions.NONE)
                return (Hit.NO_HIT, -1);

            if (!t.IsInJudgingRange(music.Time))
                return (Hit.NO_HIT, -1);

            timings.Dequeue();
            return t.Judge(music.Time, actions);
        }

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // caching references  ---------------------------------------------

            music = GetComponent<MusicManager>();
            if (music == null) {
                Debug.LogError("fail to get: MusicManager");
            }
            score = GetComponent<Score>();
            if (score == null) {
                Debug.LogError("fail to get: Score");
            }
            beatmap = GetComponent<Beatmap>();
            if (beatmap == null) {
                Debug.LogError("fail to get: Beatmap");
            }

            // init timings  ---------------------------------------------------
            timings = new Queue<Timing>();

            // Build full timing list
            int noteIdx = 0;
            foreach (Note note in beatmap.notesQ) {
                timings.Enqueue(new Timing(beatmap, note, noteIdx));
                noteIdx++;
            }
        }

        public void Update() {
            if (GCS.I.states != GameState.MAIN_PIECE || timings.Count == 0)
                return;

            CheckMissedByPassing();
        }

        // private members  ####################################################
        private Queue<Timing> timings;

        // Cached References
        private MusicManager music;
        private Score score;
        private Beatmap beatmap;

        // private methods  ####################################################
        public void CheckMissedByPassing() {
            while (timings.Count > 0 && timings.Peek().IsMissedByPassing(music.Time)) {
                timings.Dequeue();
                score.Record(Hit.LATE_MISS);
            }
        }

    }
}