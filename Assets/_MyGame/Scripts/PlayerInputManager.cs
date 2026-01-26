using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager {

    private readonly PlayerInput playerInput;
    private PressedActions pressedActions = PressedActions.NONE;

    public PlayerInputManager(PlayerInput playerInput) {
        this.playerInput = playerInput;
        playerInput.onActionTriggered += OnActionTriggered;
    }

    public void Dispose() {
        playerInput.onActionTriggered -= OnActionTriggered;
    }

    ~PlayerInputManager() {
        Dispose();
    }


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
        // TODO sent up C# events to be used by player script
        Debug.Log(pressedActions);  // HACK
    }
}

[Flags]
internal enum PressedActions {
    NONE = 0,
    JUMP = 1 << 0,
    DASH = 1 << 1,
    POWER_JUMP = 1 << 2,
}
