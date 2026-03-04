using UnityEngine;

using System.Collections.Generic;

namespace Assets._Achromatic.Scripts.Beatmap {

    public class Beatmap {

        // public members ######################################################

        public BeatmapMeta meta;
        public BeatmapData data;
        public Queue<BeatmapNote> notesQ;

        public Vector2 origin;
        public float currentBeatCount;
        public readonly float horizontalSpeedInMainPiece;

        // public methods  #####################################################
        public float CalcXFromBeat(float beatCount) {
            return origin.x + beatCount * meta.horizontalUnitsPerBeat;
        }

        public float CalcCurrentXFromBeat() {
            return CalcXFromBeat(currentBeatCount);
        }

        /// <returns>realtime beat count based on Audio Source time,
        /// start on <c>0.0f</c></returns>
        public float BeatCount {
            get {
                return p.music.Time * beatPerSec - preludeOffsetAsBeat;
            }
        }


        // MonoBehavior Lifecycle  #############################################

        public void Update() {
            // update current beat count
            currentBeatCount = piece.music.Time * beatsPerSecond;
        }

        // constructor  ########################################################
        public Beatmap(PieceScript piece, BeatmapMeta beatmapMeta) {
            // BUG fix
            p = piece;

            if (p.beatmap.meta == null) {
                Debug.LogError("must assign Beatmap Meta in Piece");
                return;
            }
            if (p.beatmap.meta.file == null) {
                Debug.LogError("must assign Beatmap File in Beatmap Meta");
                return;
            }

            beatmapData = JsonUtility.FromJson<BeatmapData>(p.beatmap.meta.file.text);
            if (beatmapData.notes.Length == 0) {
                Debug.LogError("beatmap file contains no notes: "
                        + beatmapMeta.file.name);
            }

            // fill notesQ
            notesQ = new();
            foreach (BeatmapData.JsonDataNote jsonNote
                    in beatmapData.notes) {
                notesQ.Enqueue(new BeatmapNote(jsonNote));
            }
            // init vars
            beatPerSec = this.beatmapMeta.tempo / 60.0f;
            preludeOffsetAsBeat = this.beatmapMeta.preludeSeconds * beatPerSec;
            beatsPerDivision = 1 / this.beatmapMeta.subdivisionPerBeat;


            this.piece = piece;
            meta = beatmapMeta;

            var pos3 = piece.mainPartPath.EvaluatePosition(0, 0f);
            origin = new Vector2(pos3.x, pos3.y);

            beatsPerSecond = meta.tempo / 60f;
            horizontalSpeedInMainPiece =
                    meta.horizontalUnitsPerBeat * beatsPerSecond;
        }


        // private members  ####################################################
        private readonly float beatsPerSecond;

        // cached references
        PieceScript piece;

        private readonly float beatPerSec;
        private readonly float preludeOffsetAsBeat;
        private readonly float beatsPerDivision;

    }
}

