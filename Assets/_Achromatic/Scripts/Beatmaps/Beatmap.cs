using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Achromatic.Scripts.Pieces;
using UnityEngine;

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

        public float PreludeSeconds => meta.preludeSeconds;

        // Public Methods  #####################################################

        public float CalcXFromBeat(float beat) {
            return origin.x + beat * meta.horizontalUnitsPerBeat;
        }

        public float CalcCurrentXFromBeat() {
            return CalcXFromBeat(currentBeat);
        }

        public float CalcNoteBeat(Note note) {
            return (note.bar - 1) * meta.beatPerBar
                + (note.beat - 1)
                + (note.subbeat - 1) * beatsPerDivision;
        }

        // Inspector Fields  ###################################################

        public BeatmapMeta meta;

        [SerializeField]
        private Transform originReferences;

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // test inspector fields  ------------------------------------------
            if (originReferences == null) {
                Debug.LogError("must assign: Origin References", this);
            }
            if (meta == null) {
                Debug.LogError("must assign: Beatmap Meta", this);
                return;
            }
            if (meta.file == null) {
                Debug.LogError(
                    "must assign Beatmap File in Beatmap Meta",
                    this
                );
                return;
            }

            // caching reference of piece  -------------------------------------
            piece = GetComponent<Piece>();
            if (piece == null) {
                Debug.LogError("fail to get: Piece", this);
            }

            music = GetComponent<MusicManager>();
            if (music == null) {
                Debug.LogError("fail to get: MusicManager", this);
            }

            // load data  ------------------------------------------------------
            data = JsonUtility.FromJson<BeatmapData>(meta.file.text);
            if (data.notes.Length == 0) {
                Debug.LogError(
                    "Beatmap:\tbeatmap file contains no notes: "
                        + meta.file.name,
                    this
                );
            }

            // init vars  ------------------------------------------------------
            beatsPerSecond = meta.tempo / 60.0f;
            secondsPerBeat = 60.0f / meta.tempo;
            preludeOffsetAsBeat = meta.preludeSeconds * beatsPerSecond;
            beatsPerDivision = 1 / meta.subdivisionPerBeat;
            origin = new Vector2(
                originReferences.position.x,
                originReferences.position.y
            );

            Debug.Log("beatmap origin: " + origin);

            // todo use speed mux
            speedXInMainPiece = meta.horizontalUnitsPerBeat * beatsPerSecond;

            // fill notesQ  ----------------------------------------------------
            notesQ = new();
            foreach (Note note in data.notes) {
                notesQ.Enqueue(note);
            }
        }

        private void Update() {
            // update current beat count
            currentBeat = music.Time * beatsPerSecond - preludeOffsetAsBeat;
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
