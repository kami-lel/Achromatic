using UnityEngine;

using Assets._Achromatic.Scripts.Players;


namespace Assets._Achromatic.Scripts.Pieces {

    public class Ender: MonoBehaviour {


        // Inspector Fields  ###################################################

        [SerializeField]
        private AudioSource preludeAndMain;

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // caching references of player  -----------------------------------
            player = GCS.FindPlayer().GetComponent<Player>();
        }


        private void Update() {
            if ((GCS.I.states & GameState.PIECE_CONTROl) != 0 &&
                    !preludeAndMain.isPlaying) {

                GCS.I.states = GameState.PIECE_FINISHED;
                EndPiece();
            }
        }


        // private members  ####################################################
        // Cached References
        private Player player;

        // private methods  ####################################################

        private void EndPiece() {
            Debug.Log("abc");  // HACK
        }
    }

}