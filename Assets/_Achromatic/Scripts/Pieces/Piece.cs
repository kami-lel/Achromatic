
using UnityEngine;
using UnityEngine.Splines;

namespace Assets._Achromatic.Scripts.Pieces {
    public class Piece: MonoBehaviour {

        // public members  #####################################################

        public Vector2 preludeStartOrigin;  // TODO assign


        // Inspector Fields  ###################################################
        public SplineContainer mainPath;

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // test inspector fields  ------------------------------------------
            if (mainPath == null) {
                Debug.LogError("must assign: Main Path");
            }
        }
    }
}