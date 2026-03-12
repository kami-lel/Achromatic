using System;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;
using UnityEngine.InputSystem;


public class SFX: MonoBehaviour {

    // todo randomize b/t different samples
    // todo audio cue to reflects both judge result & action type

    // public members  =========================================================

    // singleton
    public static SFX I {
        get; private set;
    }

    // public methods  =========================================================

    public void Jump(Hit hit = Hit.NONE) {
        jumpSFX.Play();
        jumpSFX.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble("Jump", hit);
    }

    public void Squat(Hit hit = Hit.NONE) {
        dashSFX.Play();
        dashSFX.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble("Squat", hit);
    }

    public void Attack(Hit hit = Hit.NONE) {
        dashSFX.Play();
        dashSFX.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble("Attack", hit);
    }


    // Inspector Fields  =======================================================
    [SerializeField]
    private AudioSource jumpSFX;

    [SerializeField]
    private AudioSource dashSFX;

    // MonoBehavior Lifecycle  =================================================
    private void Awake() {
        if (I == null) {  // create Singleton
            I = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        if (I != this) {  // guard against duplicate
            Debug.LogError("SFX:\tplace SFXManager Prefab only in 1st scene");
            Destroy(gameObject);
        }
    }

    // controller rumbling  ====================================================

    private void PlayRumble(String action, Hit hit = Hit.NONE) {
        var pad = Gamepad.current;
        if (pad == null) {
            return;
        }

        // set rumble data  ----------------------------------------------------
        float low, high, duration;
        if ((hit & Hit.PERFECT) != 0 || hit == Hit.NONE) {
            switch (action) {
            case "Jump":
            default:
                low = 0.35f;
                high = 1.00f;
                duration = 0.14f;
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
            }
        } else if ((hit & Hit.GREAT) != 0) {
            low = 0.55f;
            high = 0.65f;
            duration = 0.12f;
        } else if ((hit & Hit.GOOD) != 0) {
            low = 0.35f;
            high = 0.40f;
            duration = 0.10f;
        } else {  // miss
            low = 0.90f;
            high = 0.20f;
            duration = 0.18f;
        }

        // perform rumble  -----------------------------------------------------
        pad.SetMotorSpeeds(low, high);  // start motors
        _ = StartCoroutine(StopRumbleAfter(pad, duration));
    }

    private System.Collections.IEnumerator StopRumbleAfter(
            Gamepad pad, float duration) {
        yield return new WaitForSeconds(duration);

        pad?.SetMotorSpeeds(0f, 0f);   // stop motors
    }
}
