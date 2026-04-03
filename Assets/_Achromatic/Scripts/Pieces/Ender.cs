using UnityEngine;

using Assets._Achromatic.Scripts.Players;
using Cinemachine;
using Assets._Achromatic.Scripts.UI;


namespace Assets._Achromatic.Scripts.Pieces {

    public class Ender: MonoBehaviour {


        // Inspector Fields  ###################################################

        [SerializeField]
        private Transform playerTransform;

        [SerializeField]
        private float endX;

        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        [SerializeField]
        private GameObject finalPointWindow;

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // test inspector fields  ------------------------------------------
            if (virtualCamera == null) {
                Debug.LogError("must assign: Virtual Camera", this);
            }
            if (finalPointWindow == null) {
                Debug.LogError("must assign: Final Point Window", this);
            }

            // caching references of player  -----------------------------------
            GameObject playerGO = GCS.FindPlayer();

            player = playerGO.GetComponent<Player>();

            pim = playerGO.GetComponent<Players.InputManager>();
            if (pim == null) {
                Debug.LogError("fail to get: Player InputManager", this);
            }

            rb = playerGO.GetComponent<Rigidbody2D>();
            if (rb == null) {
                Debug.LogError("fail to find: Rigidbody2D", this);
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
        private Rigidbody2D rb;
        private Players.InputManager pim;

        // private methods  ####################################################

        private void EndPiece() {
            pim.SetInputForExplorePlay();
            rb.bodyType = RigidbodyType2D.Dynamic;
            GCS.I.states = GameState.EXPLORE_CONTROL;
            virtualCamera.Priority = 0;
            finalPointWindow.SetActive(true);
            Debug.Log("End Piece");
        }
    }

}