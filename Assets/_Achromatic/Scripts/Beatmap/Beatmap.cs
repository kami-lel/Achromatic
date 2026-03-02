using UnityEngine;


namespace Assets._Achromatic.Scripts.Beatmap {

    public class Beatmap {

        // public members ######################################################

        public BeatmapMeta meta;
        public BeatmapData data;
        public Vector2 origin;
        public float currentBeatCount;

        // public methods  #####################################################
        public float CalcXFromBeat(float beatCount) {
            return origin.x + beatCount * meta.horizontalSpeedPerBeat;
        }

        public float CalcCurrentXFromBeat() {
            return CalcXFromBeat(currentBeatCount);
        }


        // MonoBehavior Lifecycle  #################################################
        public void Update() {
            // TODO update current beat count
        }


        // constructor  ########################################################
        public Beatmap(PieceScript piece) {
            this.piece = piece;

            var pos3 = piece.mainPartPath.EvaluatePosition(0, 0f);
            origin = new Vector2(pos3.x, pos3.y);
        }


        // private members  ####################################################
        // cached references
        PieceScript piece;

    }
}

