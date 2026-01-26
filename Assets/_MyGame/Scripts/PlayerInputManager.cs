using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerInputManager {
    private readonly PlayerInput playerInput;
    private PressedActions pressedActions = PressedActions.NONE;

    public PlayerInputManager(PlayerInput playerInput) {
        this.playerInput = playerInput;
        playerInput.onActionTriggered += OnActionTriggered;
    }

    public void Unsubscribe() {
        playerInput.onActionTriggered -= OnActionTriggered;
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
                // TODO
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

        Debug.Log(pressedActions); // HACK
    }
}

[Flags]
public enum PressedActions {
    NONE = 0,
    JUMP = 1 << 0,
    DASH = 1 << 1,
    POWER_JUMP = 1 << 2,
}
