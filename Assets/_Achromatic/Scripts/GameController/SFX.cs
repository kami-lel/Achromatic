using System;
using Assets._Achromatic.Scripts.Players;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;
using UnityEngine.InputSystem;


// Fixme new action sfx

public class SFX: MonoBehaviour {

    // Public Members  #########################################################

    // singleton
    public static SFX I {
        get; private set;
    }


    // Public Methods  #########################################################

    public void OnHit(Actions pressed, Hit hit) {
        if ((hit & Hit.PERFECT) != 0) {
            // perfect, use action sound
            if ((pressed & Actions.JUMP) != 0) {
                Jump();
            } else if ((pressed & Actions.SQUAT) != 0) {
                Squat();

            } else if ((pressed & Actions.ATTACK) != 0) {
                Attack();
            }

        } else if ((hit & Hit.GREAT) != 0) {
            PlaySFX(greatSFX);
            PlayRumble("Great");

        } else if ((hit & Hit.GOOD) != 0) {
            PlaySFX(goodSFX);
            PlayRumble("Good");

        } else if ((hit & (Hit.MISS | Hit.INCORRECT)) != 0) {
            PlaySFX(missSFX1);
            PlayRumble("Miss");

        } else {
            PlaySFX(missSFX2);
            PlayRumble("Miss");

        }
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

        PlayRumble("Jump");
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

        PlayRumble("Land");

    }

    public void Squat() {
        PlaySFX(dashSFX1);
        PlayRumble("Squat");
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
    [SerializeField] private AudioSource greatSFX;
    [SerializeField] private AudioSource goodSFX;
    [SerializeField] private AudioSource missSFX1;
    [SerializeField] private AudioSource missSFX2;

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        I = this;
    }


    // constants  ##############################################################
    private const float SFX_LASTING_TIME = 1.0f;

    // private methods  ########################################################

    private void PlaySFX(AudioSource src) {
        src.Play();
        src.SetScheduledEndTime(AudioSettings.dspTime + SFX_LASTING_TIME);
    }

    private void Attack(Hit hit = Hit.NONE) {
        PlaySFX(attackSFX1);
        PlayRumble("Attack");
    }

    // ramble  =================================================================

    private void PlayRumble(String rambleType) {
        var pad = Gamepad.current;
        if (pad == null) {
            return;
        }

        float low, high, duration;

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
        pad.SetMotorSpeeds(low, high);  // start motors
        _ = StartCoroutine(StopRumbleAfter(pad, duration));
    }

    private System.Collections.IEnumerator StopRumbleAfter(Gamepad pad, float duration) {
        yield return new WaitForSeconds(duration);

        pad?.SetMotorSpeeds(0f, 0f);   // stop motors
    }
}
