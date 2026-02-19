using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameControllerScript: MonoBehaviour {

    // Inspector Fields  #######################################################

    [SerializeField]
    private GameObject player;

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

}
