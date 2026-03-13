using UnityEngine;

using System.Collections.Generic;
using Assets._Achromatic.Scripts.Pieces;
using System;
using System.Linq;

namespace Assets._Achromatic.Scripts.Beatmaps {
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(MusicManager))]
    [RequireComponent(typeof(Piece))]
    public class Beatmap: MonoBehaviour {

        // Public Members ######################################################

        [NonSerialized]
        public Queue<Note> notesQ;

        [NonSerialized]
        public Vector2 origin;

        [NonSerialized]
        public float currentBeat;

        [NonSerialized]
        public float speedXInMainPiece;

        [NonSerialized]
        public float beatsPerSecond;

        [NonSerialized]
        public float secondsPerBeat;

        // Public Methods  #####################################################

        /// <returns>realtime beat count based on Audio Source time,
        /// start on <c>0.0f</c></returns>
        public float BeatCount {
            get {
                return music.Time * beatsPerSecond - preludeOffsetAsBeat;
            }
        }

        public int NotesCount {
            get {
                return data.notes.Count();
            }
        }

        public float CalcXFromBeat(float beatCount) {
            return origin.x + beatCount * meta.horizontalUnitsPerBeat;
        }

        public float CalcCurrentXFromBeat() {
            return CalcXFromBeat(currentBeat);
        }

        public float CalcBeatCount(Note note) {
            return (note.bar - 1) * meta.beatPerBar
                    + (note.beat - 1)
                    + (note.subbeat - 1) * beatsPerDivision;
        }

        // Inspector Fields  ###################################################

        public BeatmapMeta meta;

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // test inspector fields  ------------------------------------------
            if (meta == null) {
                Debug.LogError("must assign: Beatmap Meta");
                return;
            }
            if (meta.file == null) {
                Debug.LogError("must assign Beatmap File in Beatmap Meta");
                return;
            }

            // caching reference of piece  -------------------------------------
            piece = GetComponent<Piece>();
            if (piece == null) {
                Debug.LogError("fail to get: Piece");
            }

            music = GetComponent<MusicManager>();
            if (music == null) {
                Debug.LogError("fail to get: MusicManager");
            }

            // load data  ------------------------------------------------------
            data = JsonUtility.FromJson<BeatmapData>(meta.file.text);
            if (data.notes.Length == 0) {
                Debug.LogError("Beatmap:\tbeatmap file contains no notes: "
                        + meta.file.name);
            }

            // init vars  ------------------------------------------------------
            beatsPerSecond = meta.tempo / 60.0f;
            secondsPerBeat = 60.0f / meta.tempo;
            preludeOffsetAsBeat = meta.preludeSeconds * beatsPerSecond;
            beatsPerDivision = 1 / meta.subdivisionPerBeat;

            var pos3 = piece.mainPath.EvaluatePosition(0, 0f);
            origin = new Vector2(pos3.x, pos3.y);

            // todo use speed mux
            speedXInMainPiece =
                    meta.horizontalUnitsPerBeat * beatsPerSecond;

            // fill notesQ  ----------------------------------------------------
            notesQ = new();
            foreach (Note note in data.notes) {
                notesQ.Enqueue(note);
            }
        }

        private void Update() {
            // update current beat count
            currentBeat = music.Time * beatsPerSecond;
        }


        // private members  ####################################################
        private BeatmapData data;

        private float preludeOffsetAsBeat;
        private float beatsPerDivision;

        // cached references
        private MusicManager music;
        private Piece piece;
    }
}

