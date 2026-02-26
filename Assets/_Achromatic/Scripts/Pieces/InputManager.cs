namespace Assets._Achromatic.Scripts.Pieces {

    /// <summary>
    /// manage input during music piece
    /// </summary>
    public class InputManager {

        public InputManager() {

        }
        // TODO

        //     private InputPressedActions pressedActions;

        //     private void OnActionTriggered(InputAction.CallbackContext ctxt) {
        //         InputAction a = ctxt.action;

        //         switch (a.phase) {
        //         case InputActionPhase.Started:
        //             switch (a.name) {
        //             case "Jump":
        //                 pressedActions |= InputPressedActions.JUMP;
        //                 break;
        //             case "Dash":
        //                 pressedActions |= InputPressedActions.DASH;
        //                 break;
        //             case "PowerJump":
        //                 pressedActions |= InputPressedActions.POWER_JUMP;
        //                 break;
        //             case "Trigger":
        //                 InputTrigger();
        //                 break;
        //             }
        //             break;

        //         case InputActionPhase.Canceled:
        //             switch (a.name) {
        //             case "Jump":
        //                 pressedActions &= ~InputPressedActions.JUMP;
        //                 break;
        //             case "Dash":
        //                 pressedActions &= ~InputPressedActions.DASH;
        //                 break;
        //             case "PowerJump":
        //                 pressedActions &= ~InputPressedActions.POWER_JUMP;
        //                 break;
        //             }
        //             break;
        //         }
        //     }

        //     private void InputTrigger() {
        //         Hit judgeResult = judgeCriteria.Judge(
        //                 audioSource.time, pressedActions);
        //         scoreTracker.Record(judgeResult);

        //         // control player  -----------------------------------------------------
        //         if ((pressedActions & InputPressedActions.JUMP) != 0) {
        //             playerScript.AnimationJump();
        //         } else if ((pressedActions & InputPressedActions.DASH) != 0) {
        //             playerScript.AnimationDash();
        //         }

        //         GameControllerScript.Instance.tmpUpdateText(judgeResult,
        //                 scoreTracker.combo,
        //                 scoreTracker.runningScore);
        //         // Todo add audio for feedback
        //     }

        //     private void InputStart() {
        //         playerInput.onActionTriggered += OnActionTriggered;
        //         pressedActions = InputPressedActions.NONE;
        //     }
    }
}

