using UnityEngine;

using Assets._Achromatic.Scripts.Players;


namespace Assets._Achromatic.Scripts.Pieces {

    public class Ender: MonoBehaviour {


        // Inspector Fields  ###################################################

        [SerializeField]
        private Transform playerTransform;

        [SerializeField]
        private float endX;

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // caching references of player  -----------------------------------
            GameObject playerGO = GCS.FindPlayer();

            player = playerGO.GetComponent<Player>();

            pim = playerGO.GetComponent<Players.InputManager>();
            if (pim == null) {
                Debug.LogError("fail to get: Player InputManager");
            }
        }


        private void Update() {
            if ((GCS.I.states & GameState.MAIN_PIECE) != 0 &&
                        playerTransform.position.x > endX) {
                // fixme better logic to trigger ending


                GCS.I.states = GameState.PIECE_FINISHED;
                EndPiece();
            }
        }


        // private members  ####################################################
        // Cached References
        private Player player;
        private Players.InputManager pim;

        // private methods  ####################################################

        private void EndPiece() {
            pim.SetInputForExplorePlay();
            Debug.Log("AABBCC");  // HACK
        }
    }

}