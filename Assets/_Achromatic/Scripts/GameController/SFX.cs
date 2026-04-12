using System;
using System.Collections;
using System.Collections.Generic;
using Assets._Achromatic.Scripts.Players;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;


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
            greatSFX.Play();
            Rumbler.I.Rumble("Great");

        } else if ((hit & Hit.GOOD) != 0) {
            goodSFX.Play();
            Rumbler.I.Rumble("Good");

        } else if ((hit & (Hit.MISS | Hit.INCORRECT)) != 0) {
            missSFX1.Play();
            Rumbler.I.Rumble("Miss");

        } else {
            missSFX2.Play();
            Rumbler.I.Rumble("Miss");

        }
    }

    // directly play action-audio  =============================================

    public void Jump() {
        PlayOneOfRandomSFX(jumpSFXs);

        Rumbler.I.Rumble("Jump");
    }

    public void Land() {
        PlayOneOfRandomSFX(landSFXs);

        Rumbler.I.Rumble("Land");

    }

    public void Squat() {
        dashSFX1.Play();

        Rumbler.I.Rumble("Squat");
    }

    public void StartRun() {
        if (isPlayingRun) {
            return;
        }
        playCoroutine = StartCoroutine(PlayRunLoop());
    }

    public void StopRun() {
        if (!isPlayingRun) {
            return;
        }

        StopCoroutine(playCoroutine);
        playCoroutine = null;
        isPlayingRun = false;
    }

    // Inspector Fields  #######################################################

    [Header("Action SFX")]

    [SerializeField] private AudioSource[] jumpSFXs;

    [SerializeField] private AudioSource[] landSFXs;

    [SerializeField] private AudioSource[] runSFXs;

    [SerializeField] private AudioSource dashSFX1;
    [SerializeField] private AudioSource dashSFX2;
    [SerializeField] private AudioSource dashSFX3;

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
    private const float RUN_SFX_INTERVAL = 1.0f;


    // private members  ########################################################
    private bool isPlayingRun = false;
    private Coroutine playCoroutine;

    // private methods  ########################################################

    private void PlayOneOfRandomSFX(AudioSource[] audioSources) {
        if (audioSources == null || audioSources.Length == 0) {
            Debug.LogWarning("audioSources null or empty");
            return;
        }

        int i = UnityEngine.Random.Range(0, audioSources.Length);
        audioSources[i].PlayOneShot(audioSources[i].clip);
    }

    private void Attack(Hit hit = Hit.NONE) {
        attackSFX1.Play();
        Rumbler.I.Rumble("Attack");
    }

    private IEnumerator PlayRunLoop() {
        isPlayingRun = true;
        while (true) {
            PlayOneOfRandomSFX(runSFXs);
            yield return new WaitForSeconds(RUN_SFX_INTERVAL);
        }
    }
}
