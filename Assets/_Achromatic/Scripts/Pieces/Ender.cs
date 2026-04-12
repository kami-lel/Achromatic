using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Assets._Achromatic.Scripts.Pieces {
    [RequireComponent(typeof(PlayerManager))]
    public class Ender: MonoBehaviour {
        // Public Methods  #####################################################

        public void FinishMain() {
            Debug.Log("Finish Main", this);

            GCS.I.states = GameState.TOTAL_SCORE_WINDOW;

            playerManager.FinishMain();

            virtualCamera.Priority = 0;
            midWall.SetActive(true);

            // re final point window  ------------------------------------------
            finalPointWindow.SetActive(true);

            // close after certain time
            StartCoroutine(DeactivateAfterDelay());

            // close on any key press
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
        private float finalPointWindowAutoCloseSecond = 15f;

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
            playerManager = GetComponent<PlayerManager>();
            if (playerManager == null) {
                Debug.LogError("fail to get: Player Manager", this);
            }

            GameObject go = GameController.I.FindMainPlayer();
            pi = go.GetComponent<PlayerInput>();

            if (pi == null) {
                Debug.LogError("fail to find: Player Input", this);
            }
        }

        private void OnDisable() {
            pi.onActionTriggered -= OnPressAnyKey;
        }

        // private members  ####################################################
        // Cached References
        private PlayerManager playerManager;
        private PlayerInput pi;

        // private methods  ####################################################

        private IEnumerator DeactivateAfterDelay() {
            yield return new WaitForSeconds(finalPointWindowAutoCloseSecond);
            CloseFinalPointWindow();
        }

        private void OnPressAnyKey(InputAction.CallbackContext ctxt) {
            CloseFinalPointWindow();
            pi.onActionTriggered -= OnPressAnyKey;
        }

        private void CloseFinalPointWindow() {
            GCS.I.states = GameState.EXPLORE;
            finalPointWindow.SetActive(false);
        }
    }
}
