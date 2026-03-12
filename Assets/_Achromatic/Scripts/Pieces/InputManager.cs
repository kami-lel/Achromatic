
using UnityEngine;
using UnityEngine.InputSystem;
using Assets._Achromatic.Scripts.Scores;


namespace Assets._Achromatic.Scripts.Pieces {

    /// <summary>
    /// manage input during music piece
    /// </summary>
    public class InputManager {

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
                case "Dash":
                    pressed |= PressedActions.DASH;
                    break;
                case "PowerJump":
                    pressed |= PressedActions.POWER_JUMP;
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
                case "Dash":
                    pressed &= ~PressedActions.DASH;
                    break;
                case "PowerJump":
                    pressed &= ~PressedActions.POWER_JUMP;
                    break;
                }
                break;
            }
        }

        private void Trigger() {
            Hit judgeResult = p.criteria.Judge(pressed);
            p.score.Record(judgeResult);

            if ((pressed & PressedActions.JUMP) != 0) {
                p.playerManager.tmpJump();
                p.playerManager.player.AnimationJump();
            } else if ((pressed & PressedActions.DASH) != 0) {
                p.playerManager.player.AnimationDash();
            }

            // Todo add audio for feedback, layered
        }


    }

}

