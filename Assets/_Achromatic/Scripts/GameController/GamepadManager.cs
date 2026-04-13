using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

[RequireComponent(typeof(GameController))]
public class GamepadManager: MonoBehaviour {
    // Public Members  #########################################################

    // singleton
    public static GamepadManager I {
        get; private set;
    }

    public bool isUsingGamepad = false;

    public event Action OnSwitchToGamepad;
    public event Action OnSwitchToKeyboard;

    // Public Methods  #########################################################

    public void Rumble(string rambleType) {
        var pad = Gamepad.current;
        if (!isUsingGamepad || pad == null) {
            return;
        }

        float low,
            high,
            duration;

        switch (rambleType) {
        case "Jump":
        default:
            low = 0.35f;
            high = 1.00f;
            duration = 0.14f;
            break;
        case "Land":
            low = 0.50f;
            high = 0.60f;
            duration = 0.10f;
            break;
        case "Attack":
            low = 1.00f;
            high = 0.80f;
            duration = 0.16f;
            break;
        case "Squat":
            low = 0.70f;
            high = 0.30f;
            duration = 0.15f;
            break;
        case "Great":
            low = 0.55f;
            high = 0.65f;
            duration = 0.12f;
            break;
        case "Good":
            low = 0.35f;
            high = 0.40f;
            duration = 0.10f;
            break;
        case "Miss":
            low = 0.90f;
            high = 0.20f;
            duration = 0.20f;
            break;
        }

        // perform rumble  -----------------------------------------------------
        pad.SetMotorSpeeds(low, high); // start motors
        _ = StartCoroutine(StopRumbleAfter(pad, duration));
    }

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        I = this;
    }

    private void OnEnable() {
        InputSystem.onEvent += HandleInputEvent;
    }

    private void OnDisable() {
        InputSystem.onEvent -= HandleInputEvent;
    }

    // Event Handler  ##########################################################
    private void HandleInputEvent(InputEventPtr eventPtr, InputDevice device) {
        if (
            (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
            || device.CheckStateIsAtDefault()
        ) {
            return;
        }

        bool currentIsUsingGamepad = device is Gamepad;

        if (currentIsUsingGamepad != isUsingGamepad) {
            // HACK
            if (currentIsUsingGamepad) {
                OnSwitchToGamepad?.Invoke();
                Debug.Log("Switch to: Gamepad", this);
            } else {
                OnSwitchToKeyboard?.Invoke();
                Debug.Log("Switch to: Keyboard", this);
            }
        }

        isUsingGamepad = currentIsUsingGamepad;
    }

    // private methods  ########################################################

    private System.Collections.IEnumerator StopRumbleAfter(
        Gamepad pad,
        float duration
    ) {
        yield return new WaitForSeconds(duration);

        pad?.SetMotorSpeeds(0f, 0f); // stop motors
    }
}
