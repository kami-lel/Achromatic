using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager {

    private readonly PlayerInput playerInput;
    private PressedActions pressedActions;

    public PlayerInputManager(PlayerInput playerInput) {
        this.playerInput = playerInput;
        playerInput.onActionTriggered += OnActionTriggered;

        pressedActions = PressedActions.NONE;
    }

    public void Dispose() {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    ~PlayerInputManager() {
        Dispose();
    }

    /// <summary>
    /// called by Input Actions
    /// </summary>
    /// <param name="ctxt"></param>
    private void OnActionTriggered(InputAction.CallbackContext ctxt) {
        InputAction a = ctxt.action;

        switch (a.phase) {
        case InputActionPhase.Started:
            switch (a.name) {
            case "Jump":
                pressedActions |= PressedActions.JUMP;
                break;
            case "Dash":
                pressedActions |= PressedActions.DASH;
                break;
            case "PowerJump":
                pressedActions |= PressedActions.POWER_JUMP;
                break;
            case "Trigger":
                Trigger();
                break;
            }
            break;

        case InputActionPhase.Canceled:
            switch (a.name) {
            case "Jump":
                pressedActions &= ~PressedActions.JUMP;
                break;
            case "Dash":
                pressedActions &= ~PressedActions.DASH;
                break;
            case "PowerJump":
                pressedActions &= ~PressedActions.POWER_JUMP;
                break;
            }
            break;
        }
    }

    private void Trigger() {
        // Todo control user when appropriate
        Debug.Log(pressedActions);
    }
}

[Flags]
internal enum PressedActions {
    NONE = 0,
    JUMP = 1 << 0,
    DASH = 1 << 1,
    POWER_JUMP = 1 << 2,
}
