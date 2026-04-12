using System.Collections;
using Assets._Achromatic.Scripts.Metric;
using Assets._Achromatic.Scripts.Scores;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets._Achromatic.Scripts.Pieces {
    [RequireComponent(typeof(PlayerManager))]
    [RequireComponent(typeof(Score))]
    public class Ender: MonoBehaviour {
        // Public Methods  #####################################################

        public void FinishMain() {
            Debug.Log("Finish Main", this);

            GameController.I.states = GameState.TOTAL_SCORE_WINDOW;

            playerManager.FinishMain();

            virtualCamera.Priority = 0;
            midWall.SetActive(true);

            // re final point window  ------------------------------------------
            finalPointWindow.SetActive(true);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Metrics.I.LogMusicPlay(score);
#endif

            // close after certain time
            StartCoroutine(DeactivateAfterDelay());

            // block input for first few seconds, then allow close on any key
            isInputBlocked = true;
            StartCoroutine(UnblockInputAfterDelay());
            pi.onActionTriggered += OnPressAnyKey;
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

        [SerializeField]
        private float finalPointWindowMinTime = 1f;

        [SerializeField]
        private float finalPointWindowAutoCloseSecond = 15f;

        // MonoBehaviour Lifecycle  ############################################

        private void Awake() {
            // Inspector Assignment Guard  -------------------------------------
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

            // cache player refs  ----------------------------------------------
            playerManager = GetComponent<PlayerManager>();
            if (playerManager == null) {
                Debug.LogError("fail to get: Player Manager", this);
            }

            GameObject go = GameController.I.FindMainPlayer();
            pi = go.GetComponent<PlayerInput>();
            if (pi == null) {
                Debug.LogError("fail to find: Player Input", this);
            }

            // get component  --------------------------------------------------
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            score = GetComponent<Score>();
            if (score == null) {
                Debug.LogError("fail to find: score", this);
            }
#endif

        }

        private void OnDisable() {
            pi.onActionTriggered -= OnPressAnyKey;
        }

        // Private Members  ####################################################
        private bool isInputBlocked;

        // Cached References  --------------------------------------------------
        private PlayerManager playerManager;
        private PlayerInput pi;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        private Score score;
#endif

        // Private Methods  ####################################################

        private IEnumerator DeactivateAfterDelay() {
            yield return new WaitForSeconds(
                finalPointWindowAutoCloseSecond
            );
            CloseFinalPointWindow();
        }

        /// <summary>
        /// unblock input after <see cref="finalPointWindowMinTime"/> seconds
        /// </summary>
        private IEnumerator UnblockInputAfterDelay() {
            yield return new WaitForSeconds(finalPointWindowMinTime);
            isInputBlocked = false;
        }

        private void OnPressAnyKey(InputAction.CallbackContext ctxt) {
            if (isInputBlocked)
                return;  // block during min-time window

            CloseFinalPointWindow();
            pi.onActionTriggered -= OnPressAnyKey;
        }

        private void CloseFinalPointWindow() {
            GameController.I.states = GameState.EXPLORE;
            finalPointWindow.SetActive(false);
        }
    }
}