using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets._Achromatic.Scripts.Players {
    [RequireComponent(typeof(PlayerInput))]
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

            // HACK
            switch (ctxt.action.phase) {
            case InputActionPhase.Started:  // ---------------------------------
                switch (ctxt.action.name) {
                case "Jump":
                    // p.mvmt.Jump();
                    break;

                case "Left":
                    // p.mvmt.TurnLeft();
                    break;

                case "Right":
                    // p.mvmt.TurnRight();
                    break;

                case "Squat":
                    // p.mvmt.Squat();
                    break;

                case "Interact":
                    Debug.Log("Player:\tInteract!!!");  // todo implement explore interaction
                    break;

                }
                break;

            case InputActionPhase.Canceled:  // ------------------------------------
                switch (ctxt.action.name) {
                case "Left":
                case "Right":
                    // HACK
                    // p.mvmt.Stop();
                    break;
                }
                break;
            }
        }



        // private members  ########################################################
        // cached references
        private PlayerInput pi;
        private Collider2D col;



        // TODO

    }
}
