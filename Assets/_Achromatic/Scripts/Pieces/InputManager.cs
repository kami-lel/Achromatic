
using UnityEngine;
using UnityEngine.InputSystem;
using Assets._Achromatic.Scripts.Players;


namespace Assets._Achromatic.Scripts.Pieces {

    public class InputManager: MonoBehaviour {

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            pressed = Actions.NONE;

            GameObject go = GCS.FindPlayer();
            pi = go.GetComponent<PlayerInput>();

            if (pi == null) {
                Debug.LogError("fail to find: Player Input");
            }
        }

        private void Start() {
            if (pi != null) {
                pi.onActionTriggered += OnActionTriggered;
            } else {
                Debug.LogError("fail to subscribe");
            }
        }

        public void OnDisable() {
            if (pi != null) {
                pi.onActionTriggered -= OnActionTriggered;
            } else {
                Debug.LogError("fail to unsubscribe");
            }
        }

        // event handlers  #####################################################
        private void OnActionTriggered(InputAction.CallbackContext ctxt) {
            if ((GCS.I.states & GameState.PIECE_CONTROl) == 0) {
                return;
            }

            InputAction a = ctxt.action;

            switch (a.phase) {
            case InputActionPhase.Started:
                switch (a.name) {
                case "Jump":
                    pressed |= Actions.JUMP;
                    break;
                case "Squat":
                    pressed |= Actions.SQUAT;
                    break;
                case "Attack":
                    pressed |= Actions.ATTACK;
                    break;
                case "Trigger":
                    Trigger();
                    break;
                }
                break;

            case InputActionPhase.Canceled:
                switch (a.name) {
                case "Jump":
                    pressed &= ~Actions.JUMP;
                    break;
                case "Squat":
                    pressed &= ~Actions.SQUAT;
                    break;
                case "Attack":
                    pressed &= ~Actions.ATTACK;
                    break;
                }
                break;
            }
        }

        // private members  ####################################################
        private Actions pressed;

        // cached references
        private PlayerInput pi;

        // private methods  ####################################################

        private void Trigger() {
            /* HACK
            Hit hit = p.criteria.Judge(pressed);
            p.score.Record(hit);

            if ((pressed & Actions.JUMP) != 0) {
                p.playerManager.Jump(hit);
            } else if ((pressed & Actions.SQUAT) != 0) {
                p.playerManager.Squat(hit);
            } else if ((pressed & Actions.ATTACK) != 0) {
                p.playerManager.Attack(hit);
            }
            */

            // Todo add audio for feedback, layered
        }
    }

}

