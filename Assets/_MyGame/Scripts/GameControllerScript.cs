using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameControllerScript: MonoBehaviour {

    // Inspector Fields  #######################################################

    [SerializeField]
    private GameObject player;

    [Header("SFX")]

    [SerializeField]
    private AudioSource sfxJump;

    [SerializeField]
    private AudioSource sfxDash;

    [SerializeField]
    private TMPro.TextMeshProUGUI tmpJudgeResult;

    [SerializeField]
    private TMPro.TextMeshProUGUI tmpCombo;

    [SerializeField]
    private AnimationCurve tmpTextboxCurve;

    // public members  #########################################################

    /// <summary>
    /// singleton instance of <c>GameControllerScript</c>
    /// </summary>
    public static GameControllerScript Instance;  // singleton

    [NonSerialized]
    public PlayerScript playerScript;

    private float lastTriggerTime;

    // class method  ###########################################################
    /// <returns>singleton player</returns>
    public static GameObject GetPlayer() {
        if (Instance == null) {
            Debug.LogError("GameControllerScript: Instance is null");
        }

        return Instance.player;
    }

    // MonoBehavior Lifecycle  #################################################
    private void Awake() {
        // singleton single instance
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            // avoid duplicates
            Debug.LogError(
                    "GameControllerScript: Duplicate instance:"
                    + gameObject.name);
            Destroy(gameObject);
        }

        // reference to playerScript
        playerScript = player.GetComponent<PlayerScript>();

        // disable textbox
        tmpCombo.gameObject.SetActive(false);
        tmpJudgeResult.gameObject.SetActive(false);
    }

    private void Update() {
        float scale = tmpTextboxCurve.Evaluate(Time.time - lastTriggerTime);
        tmpJudgeResult.transform.localScale = new Vector3(scale, scale);
    }

    // public methods  #########################################################
    // hack tmp method
    public void tmpUpdateText(
            JudgeResult judgeResult, int combo, int runningScore) {
        if (!tmpCombo.gameObject.activeSelf) {
            tmpCombo.gameObject.SetActive(true);
        }
        if (!tmpJudgeResult.gameObject.activeSelf) {
            tmpJudgeResult.gameObject.SetActive(true);
        }


        tmpCombo.text = $"{combo} hits\nscore:{runningScore}";

        string judgeText;

        if ((judgeResult & JudgeResult.PERFECT) != 0) {
            judgeText = "Perfect!";
        } else {
            judgeText = judgeResult switch {
                JudgeResult.NO_HIT => "No Hit!",
                JudgeResult.INCORRECT => "Wrong!",
                JudgeResult.EARLY_MISS => "Miss! Too Early",
                JudgeResult.EARLY_GREAT => "Great! Too Early",
                JudgeResult.EARLY_GOOD => "Good! Too Early",
                JudgeResult.LATE_MISS => "Miss! Too Late",
                JudgeResult.LATE_GREAT => "Great! Too Late",
                JudgeResult.LATE_GOOD => "Good! Too Late",
                _ => null
            };
        }

        tmpJudgeResult.text = judgeText;

        lastTriggerTime = Time.time;
    }


    // sfx  ====================================================================

    // todo randomize b/t different samples
    // Todo rumble control as its own script
    // Todo rumble fine tuning data
    // todo rumble to reflects both judge result & action type
    // todo audio cue to reflects both judge result & action type

    public void PlayJump() {
        sfxJump.Play();
        sfxJump.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble(0.1f, 0.8f, 0.1f);
    }

    public void PlayDash() {
        sfxDash.Play();
        sfxDash.SetScheduledEndTime(AudioSettings.dspTime + 1.0f);
        PlayRumble(0.7f, 0.1f, 0.2f);
    }

    // private methods  ########################################################
    public void PlayRumble(float low, float high, float duration) {
        var pad = Gamepad.current;
        if (pad == null) {
            return;
        }

        pad.SetMotorSpeeds(low, high);  // start motors
        StartCoroutine(StopRumbleAfter(pad, duration));
    }

    private System.Collections.IEnumerator StopRumbleAfter(
            Gamepad pad, float duration) {
        yield return new WaitForSeconds(duration);
        if (pad != null)
            pad.SetMotorSpeeds(0f, 0f);  // stop motors
    }


}
