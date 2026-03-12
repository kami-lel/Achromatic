using System;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;
using UnityEngine.InputSystem;


public class SFXManager: MonoBehaviour {

    // todo randomize b/t different samples
    // todo audio cue to reflects both judge result & action type

    // public members  =========================================================

    // singleton
    public static SFXManager I {
        get; private set;
    }

    // public methods  =========================================================

    // TODO utilize the hit based actions

    public void Jump(Hit hit = Hit.NONE) {
        jumpSFX.Play();
        jumpSFX.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble("Jump");
    }

    public void Squat(Hit hit = Hit.NONE) {
        dashSFX.Play();
        dashSFX.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble("Squat");
    }

    public void Attack(Hit hit = Hit.NONE) {
        dashSFX.Play();
        dashSFX.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble("Attack");
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
                low = 0.20f;
                high = 0.85f;
                duration = 0.07f;
                break;

            case "Attack":
                low = 0.75f;
                high = 0.55f;
                duration = 0.09f;
                break;

            case "Squat":
                low = 0.45f;
                high = 0.20f;
                duration = 0.08f;
                break;
            }

        } else if ((hit & Hit.GREAT) != 0) {
            low = 0.35f;
            high = 0.45f;
            duration = 0.06f;
        } else if ((hit & Hit.GOOD) != 0) {
            low = 0.20f;
            high = 0.25f;
            duration = 0.05f;
        } else {  // miss
            low = 0.70f;
            high = 0.10f;
            duration = 0.12f;
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
