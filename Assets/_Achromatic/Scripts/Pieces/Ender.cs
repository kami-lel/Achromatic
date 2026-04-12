using Assets._Achromatic.Scripts.Players;
using Cinemachine;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {
    public class Ender: MonoBehaviour {
        // Public Methods  #####################################################

        public void FinishMain() {
            Debug.Log("Finish Main", this);

            GCS.I.states = GameState.TOTAL_SCORE_WINDOW;

            playerManager.FinishMain();

            virtualCamera.Priority = 0;
            finalPointWindow.SetActive(true);
            midWall.SetActive(true);
        }

        // Inspector Fields  ###################################################

        [SerializeField]
        private Transform playerTransform;

        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        [SerializeField]
        private GameObject finalPointWindow;

        [SerializeField]
        private GameObject midWall;

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // test inspector fields  ------------------------------------------
            if (virtualCamera == null) {
                Debug.LogError("must assign: Virtual Camera", this);
            }
            if (finalPointWindow == null) {
                Debug.LogError("must assign: Final Point Window", this);
            }

            if (midWall == null) {
                Debug.LogError("must assign: Mid Wall", this);
            }
            midWall.SetActive(false);

            // caching references of player  -----------------------------------
            GameObject playerGO = GameController.I.FindMainPlayer();

            playerManager = playerGO.GetComponent<PlayerManager>();
            if (playerManager == null) {
                Debug.LogError("fail to get: Player Manager", this);
            }
        }

        // private members  ####################################################
        // Cached References
        private PlayerManager playerManager;
    }
}
