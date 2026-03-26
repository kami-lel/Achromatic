
using System;
using UnityEngine;
using UnityEngine.Splines;

namespace Assets._Achromatic.Scripts.Pieces {
    public class Piece: MonoBehaviour {

        // public members  #####################################################

        [NonSerialized]
        public Vector2 preludeStartOrigin;

        // Inspector Fields  ###################################################
        public SplineContainer mainPath;

        [SerializeField]
        private Transform startPreludeTransform;

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // test inspector fields  ------------------------------------------
            if (mainPath == null) {
                Debug.LogError("must assign: Main Path");
            }
            if (startPreludeTransform == null) {
                Debug.LogError("must assign: Start Prelude Transform");
            }

            // caching references  ---------------------------------------------
            preludeStartOrigin = startPreludeTransform.position;
        }
    }
}