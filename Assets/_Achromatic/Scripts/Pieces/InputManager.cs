
using UnityEngine;
using UnityEngine.InputSystem;
using Assets._Achromatic.Scripts.Scores;


namespace Assets._Achromatic.Scripts.Pieces {

    public class InputManager: MonoBehaviour {
        // FIXME FIXME make it mono behavior

        public PressedActions pressed;

        public InputManager(PieceScript pieceScript) {
            p = pieceScript;

            pressed = PressedActions.NONE;

            PlayerInput playerInput = p.playerManager.playerInput;
            if (playerInput == null) {
                Debug.LogWarning("fInput:\tail to subscribe playerInput.onActionTriggered");
            } else {
                playerInput.onActionTriggered += OnActionTriggered;
            }
        }

        public void OnDisable() {
            PlayerInput playerInput = p.playerManager.playerInput;
            if (playerInput == null) {
                Debug.LogWarning("Input:\tfail to subscribe playerInput.onActionTriggered");
            } else {
                playerInput.onActionTriggered -= OnActionTriggered;
            }
        }

        private readonly PieceScript p;

        private void OnActionTriggered(InputAction.CallbackContext ctxt) {
            if ((GCS.I.states & GameState.PIECE_CONTROl) == 0) {
                return;
            }

            InputAction a = ctxt.action;

            switch (a.phase) {
            case InputActionPhase.Started:
                switch (a.name) {
                case "Jump":
                    pressed |= PressedActions.JUMP;
                    break;
                case "Squat":
                    pressed |= PressedActions.SQUAT;
                    break;
                case "Attack":
                    pressed |= PressedActions.ATTACK;
                    break;
                case "Trigger":
                    Trigger();
                    break;
                }
                break;

            case InputActionPhase.Canceled:
                switch (a.name) {
                case "Jump":
                    pressed &= ~PressedActions.JUMP;
                    break;
                case "Squat":
                    pressed &= ~PressedActions.SQUAT;
                    break;
                case "Attack":
                    pressed &= ~PressedActions.ATTACK;
                    break;
                }
                break;
            }
        }

        private void Trigger() {
            Hit hit = p.criteria.Judge(pressed);
            p.score.Record(hit);

            if ((pressed & PressedActions.JUMP) != 0) {
                p.playerManager.Jump(hit);
            } else if ((pressed & PressedActions.SQUAT) != 0) {
                p.playerManager.Squat(hit);
            } else if ((pressed & PressedActions.ATTACK) != 0) {
                p.playerManager.Attack(hit);
            }

            // Todo add audio for feedback, layered
        }


    }

}

