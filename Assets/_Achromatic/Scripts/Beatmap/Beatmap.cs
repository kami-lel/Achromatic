using UnityEngine;


namespace Assets._Achromatic.Scripts.Beatmap {

    public class Beatmap {

        // public members ######################################################

        public BeatmapMeta meta;
        public BeatmapData data;
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


        // MonoBehavior Lifecycle  #############################################

        public void Update() {
            // update current beat count
            currentBeatCount = piece.music.Time * beatsPerSecond;
        }

        // constructor  ########################################################
        public Beatmap(PieceScript piece, BeatmapMeta beatmapMeta) {
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


    }
}

