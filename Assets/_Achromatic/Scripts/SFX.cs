using System;
using Assets._Achromatic.Scripts.Players;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;
using UnityEngine.InputSystem;


// todo customize sfx for squat & attack

public class SFX: MonoBehaviour {

    // Public Members  #########################################################

    // singleton
    public static SFX I {
        get; private set;
    }


    // Public Methods  #########################################################

    public void OnHit(Actions action, Hit hit) {
        // TODO TODO on hit
    }

    // directly play action-audio  =============================================

    public void Jump() {
        switch (UnityEngine.Random.Range(0, 3)) {
        case 0:
            PlaySFX(jumpSFX1);
            break;
        case 1:
            PlaySFX(jumpSFX2);
            break;
        case 2:
            PlaySFX(jumpSFX3);
            break;
        }
    }

    public void Land() {
        switch (UnityEngine.Random.Range(0, 3)) {
        case 0:
            PlaySFX(landSFX1);
            break;
        case 1:
            PlaySFX(landSFX2);
            break;
        case 2:
            PlaySFX(landSFX3);
            break;
        }
    }

    public void Squat(Hit hit = Hit.NONE) {
        PlaySFX(dashSFX1);
    }

    // Inspector Fields  #######################################################

    [Header("Action SFX")]

    [SerializeField] private AudioSource jumpSFX1;
    [SerializeField] private AudioSource jumpSFX2;
    [SerializeField] private AudioSource jumpSFX3;

    [SerializeField] private AudioSource dashSFX1;
    [SerializeField] private AudioSource dashSFX2;
    [SerializeField] private AudioSource dashSFX3;

    [SerializeField] private AudioSource landSFX1;
    [SerializeField] private AudioSource landSFX2;
    [SerializeField] private AudioSource landSFX3;

    [SerializeField] private AudioSource attackSFX1;

    [Header("Hit SFX")]
    [SerializeField] private AudioSource perfectSFX;
    [SerializeField] private AudioSource greatSFX;
    [SerializeField] private AudioSource goodSFX;
    [SerializeField] private AudioSource missSFX;

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


    // constants  ##############################################################
    private const float SFX_LASTING_TIME = 1.0f;

    // private methods  ########################################################

    private void PlaySFX(AudioSource src) {
        src.Play();
        src.SetScheduledEndTime(AudioSettings.dspTime + SFX_LASTING_TIME);
    }


    public void Attack(Hit hit = Hit.NONE) {
        PlaySFX(attackSFX1);
    }

    // ramble  =================================================================

    // TODO use ramble

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
