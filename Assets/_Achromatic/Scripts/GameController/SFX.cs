using System;
using System.Collections.Generic;
using Assets._Achromatic.Scripts.Players;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;
using UnityEngine.InputSystem;


// FIXME new action sfx

[RequireComponent(typeof(GameController))]
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
            PlayOneShotSFX(greatSFX);
            PlayRumble("Great");

        } else if ((hit & Hit.GOOD) != 0) {
            PlayOneShotSFX(goodSFX);
            PlayRumble("Good");

        } else if ((hit & (Hit.MISS | Hit.INCORRECT)) != 0) {
            PlayOneShotSFX(missSFX1);
            PlayRumble("Miss");

        } else {
            PlayOneShotSFX(missSFX2);
            PlayRumble("Miss");

        }
    }

    // directly play action-audio  =============================================

    public void Jump() {
        PlayOneOfRandomSFX(jumpSFXs);

        PlayRumble("Jump");
    }

    public void Land() {
        switch (UnityEngine.Random.Range(0, 3)) {
        case 0:
            PlayOneShotSFX(landSFX1);
            break;
        case 1:
            PlayOneShotSFX(landSFX2);
            break;
        case 2:
            PlayOneShotSFX(landSFX3);
            break;
        }

        PlayRumble("Land");

    }

    public void Squat() {
        PlayOneShotSFX(dashSFX1);
        PlayRumble("Squat");
    }

    // Inspector Fields  #######################################################

    [Header("Action SFX")]

    [SerializeField] private AudioSource[] jumpSFXs;

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
        // singleton logic  ----------------------------------------------------
        I = this;
    }


    // constants  ##############################################################
    private const float SFX_LASTING_TIME = 1.0f;

    // private methods  ########################################################

    private void PlayOneOfRandomSFX(AudioSource[] audioSources) {
        if (audioSources == null || audioSources.Length == 0) {
            Debug.LogWarning("audioSources null or empty");
            return;
        }

        int i = UnityEngine.Random.Range(0, audioSources.Length);
        audioSources[i].PlayOneShot(audioSources[i].clip);
    }

    // HACK rm, use PlayOneShot
    private void PlayOneShotSFX(AudioSource src) {
        src.Play();
        src.SetScheduledEndTime(AudioSettings.dspTime + SFX_LASTING_TIME);
    }



    private void Attack(Hit hit = Hit.NONE) {
        PlayOneShotSFX(attackSFX1);
        PlayRumble("Attack");
    }

    // ramble  =================================================================

    // TODO make it another component

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
