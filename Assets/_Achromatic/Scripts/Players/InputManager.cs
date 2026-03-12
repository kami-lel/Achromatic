using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets._Achromatic.Scripts.Players {
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(Movement))]
    class InputManager: MonoBehaviour {
        // Public API  #########################################################
        public void SetInputForExplorePlay() {
            pi.SwitchCurrentActionMap("PlayerExplorePlay");
            col.sharedMaterial = defaultMaterial;
        }

        public void SetInputForMusicPlay() {
            pi.SwitchCurrentActionMap("PlayerMusicPlay");
            col.sharedMaterial = noFrictionMaterial;
        }

        // Public Members  #####################################################

        public event Action<string> OnTriggerEnter;
        public event Action<string> OnTriggerExit;

        // Inspector Fields  ###################################################

        [SerializeField]
        PhysicsMaterial2D defaultMaterial;

        [SerializeField]
        PhysicsMaterial2D noFrictionMaterial;

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            pi = GetComponent<PlayerInput>();
            if (pi == null) {
                Debug.LogError("InputManger:\tfail to get: PlayerInput");
            }

            col = GetComponent<Collider2D>();
            if (col == null) {
                Debug.LogError("InputManger:\tfail to get: Collider2D");
            }

            mvmt = GetComponent<Movement>();
            if (mvmt == null) {
                Debug.LogError("InputManger:\tfail to get: Movement");
            }
        }

        private void Start() {
            pi.defaultActionMap = "PlayerExplorePlay";
            SetInputForExplorePlay();
            pi.onActionTriggered += OnActionTriggered;
        }

        public void OnDisable() {
            pi.onActionTriggered -= OnActionTriggered;
        }

        // event handler  ######################################################
        private void OnActionTriggered(InputAction.CallbackContext ctxt) {
            if ((GCS.I.states & GameState.EXPLORE_CONTROL) == 0) {
                return;
            }

            switch (ctxt.action.phase) {
            case InputActionPhase.Started:  // ---------------------------------
                switch (ctxt.action.name) {
                case "Jump":
                    mvmt.Jump();
                    break;

                case "Left":
                    mvmt.TurnLeft();
                    break;

                case "Right":
                    mvmt.TurnRight();
                    break;

                case "Squat":
                    mvmt.Squat();
                    break;

                case "Interact":
                    // todo implement explore interaction
                    Debug.Log("Player:\tInteract!!!");
                    break;

                }
                break;

            case InputActionPhase.Canceled:  // --------------------------------
                switch (ctxt.action.name) {
                case "Left":
                case "Right":
                    mvmt.Stop();
                    break;
                }
                break;
            }
        }

        // private members  ####################################################
        // cached references
        private PlayerInput pi;
        private Collider2D col;
        private Movement mvmt;
    }
}
