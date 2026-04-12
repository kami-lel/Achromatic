using System.Collections;
using Assets._Achromatic.Scripts.Players;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;


[RequireComponent(typeof(GameController))]
public class SFX: MonoBehaviour {

    // Public Members  #########################################################

    // singleton
    public static SFX I {
        get; private set;
    }


    // Public Methods  #########################################################

    public void OnHit(Actions pressed, Hit hit) {
        // FIXME using the new actions
        // if ((hit & Hit.PERFECT) != 0) {
        //     // perfect, use action sound
        //     if ((pressed & Actions.JUMP) != 0) {
        //         Jump();
        //     } else if ((pressed & Actions.SQUAT) != 0) {
        //         Squat();

        //     } else if ((pressed & Actions.ATTACK) != 0) {
        //         Attack();
        //     }

        // } else if ((hit & Hit.GREAT) != 0) {
        //     greatSFX.Play();
        //     Rumbler.I.Rumble("Great");

        // } else if ((hit & Hit.GOOD) != 0) {
        //     goodSFX.Play();
        //     Rumbler.I.Rumble("Good");

        // } else if ((hit & (Hit.MISS | Hit.INCORRECT)) != 0) {
        //     missSFX1.Play();
        //     Rumbler.I.Rumble("Miss");

        // } else {
        //     missSFX2.Play();
        //     Rumbler.I.Rumble("Miss");

        // }
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
        PlayOneOfRandomSFX(squatSFXs);

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

    [SerializeField] private AudioSource[] squatSFXs;

    [Header("Hit SFX")]
    [SerializeField] private AudioSource[] greatSFXs;
    [SerializeField] private AudioSource[] goodSFXs;
    [SerializeField] private AudioSource[] missSFXs;

    [SerializeField] private AudioSource[] notHitSFXs;

    [Header("Hit SFX: Perfects")]

    [SerializeField] private AudioSource[] perfectJump;

    [SerializeField] private AudioSource[] perfectSquat;

    [SerializeField] private AudioSource[] perfectAttack;


    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // singleton logic  ----------------------------------------------------
        I = this;
    }

    // constants  ##############################################################
    private const float RUN_SFX_INTERVAL = 0.25f;


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
        // HACK
        // attackSFX1.Play();
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
