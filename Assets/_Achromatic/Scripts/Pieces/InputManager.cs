
using UnityEngine;
using UnityEngine.InputSystem;
using Assets._Achromatic.Scripts.Scores;


namespace Assets._Achromatic.Scripts.Pieces {

    /// <summary>
    /// manage input during music piece
    /// </summary>
    public class InputManager {

        public PressedActions pressed;

        public InputManager(Criteria criteria, PlayerInput playerInput) {
            // Fixme save piece as cached reference
            pressed = PressedActions.NONE;

            if (criteria == null) {
                Debug.LogWarning("fail to set criteria");
            } else {
                this.criteria = criteria;
            }

            if (playerInput == null) {
                Debug.LogWarning("fail to subscribe playerInput.onActionTriggered");
            } else {
                playerInput.onActionTriggered += OnActionTriggered;
            }
        }

        public void OnDisable(PlayerInput playerInput) {
            if (playerInput == null) {
                Debug.LogWarning("fail to subscribe playerInput.onActionTriggered");
            } else {
                playerInput.onActionTriggered -= OnActionTriggered;
            }
        }

        private readonly Criteria criteria;
        private readonly ScoreTracker scoreTracker;

        private void OnActionTriggered(InputAction.CallbackContext ctxt) {
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
            Hit judgeResult = criteria.Judge(pressed);
            scoreTracker.Record(judgeResult);

            // Fixme  control player
            // if ((pressedActions & InputPressedActions.JUMP) != 0) {
            //     playerScript.AnimationJump();
            // } else if ((pressedActions & InputPressedActions.DASH) != 0) {
            //     playerScript.AnimationDash();
            // }

            // Todo add audio for feedback, layered
        }

    }

}

