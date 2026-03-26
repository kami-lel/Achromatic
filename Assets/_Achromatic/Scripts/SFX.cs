using System;
using Assets._Achromatic.Scripts.Players;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;
using UnityEngine.InputSystem;


public class SFX: MonoBehaviour {

    // Public Members  #########################################################

    // singleton
    public static SFX I {
        get; private set;
    }


    // Public Methods  #########################################################

    public void Play(Actions actions, Hit hit = Hit.NONE) {
        // TODO generic method for play
    }

    // directly play action-audio  =============================================

    // TODO use ramble

    public void Jump() {
        switch (UnityEngine.Random.Range(0, 3)) {
        case 0:
            jumpSFX1.Play();
            jumpSFX1.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
            break;
        case 1:
            jumpSFX2.Play();
            jumpSFX2.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
            break;
        case 2:
            jumpSFX3.Play();
            jumpSFX3.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
            break;
        }
    }

    public void Land() {
        switch (UnityEngine.Random.Range(0, 3)) {
        case 0:
            landSFX1.Play();
            landSFX1.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
            break;
        case 1:
            landSFX2.Play();
            landSFX2.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
            break;
        case 2:
            landSFX3.Play();
            landSFX3.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
            break;
        }
    }

    public void Squat(Hit hit = Hit.NONE) {
        dashSFX1.Play();
        dashSFX1.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        // PlayRumble("Squat", hit);
    }

    public void Attack(Hit hit = Hit.NONE) {
        dashSFX1.Play();
        dashSFX1.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        // PlayRumble("Attack", hit);
    }



    // Inspector Fields  #######################################################
    [SerializeField] private AudioSource jumpSFX1;
    [SerializeField] private AudioSource jumpSFX2;
    [SerializeField] private AudioSource jumpSFX3;

    [SerializeField] private AudioSource dashSFX1;
    [SerializeField] private AudioSource dashSFX2;
    [SerializeField] private AudioSource dashSFX3;

    [SerializeField] private AudioSource landSFX1;
    [SerializeField] private AudioSource landSFX2;
    [SerializeField] private AudioSource landSFX3;

    // MonoBehavior Lifecycle  #################################################

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


    // private methods  ########################################################

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
            duration = 0.20f;
        }

        // perform rumble  -----------------------------------------------------
        pad.SetMotorSpeeds(low, high);  // start motors
        _ = StartCoroutine(StopRumbleAfter(pad, duration));
    }

    private System.Collections.IEnumerator StopRumbleAfter(Gamepad pad, float duration) {
        yield return new WaitForSeconds(duration);

        pad?.SetMotorSpeeds(0f, 0f);   // stop motors
    }
}
