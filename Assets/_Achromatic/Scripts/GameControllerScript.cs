using System;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;

public class GameControllerScript: MonoBehaviour {

    // singleton
    public static GameControllerScript I {
        get; private set;
    }

    public GameState gameState;

    // MonoBehavior Lifecycle  #################################################
    private void Awake() {
        if (I == null) {  // create Singleton
            I = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        if (I != this) {  // guard against duplicate
            Debug.LogError("GameController:\tplace GameController Prefab only in 1st scene");
            Destroy(gameObject);
        }


        // Hack rm these
        // reference to playerScript
        playerScript = player.GetComponent<PlayerScript>();

        // disable textbox
        tmpCombo.gameObject.SetActive(false);
        tmpJudgeResult.gameObject.SetActive(false);
    }


    // Fixme create score overlay

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


    [NonSerialized]
    public PlayerScript playerScript;

    private float lastTriggerTime;

    // class method  ###########################################################
    /// <returns>singleton player</returns>
    public static GameObject GetPlayer() {
        if (I == null) {
            Debug.LogError("GameController:\tInstance is null");
        }

        return I.player;
    }

    private void Update() {
        float scale = tmpTextboxCurve.Evaluate(Time.time - lastTriggerTime);
        // Hack
        // tmpJudgeResult.transform.localScale = new Vector3(scale, scale);
    }

    // public methods  #########################################################
    // Hack tmp method
    public void tmpUpdateText(
            Hit judgeResult, int combo, int runningScore) {
        if (!tmpCombo.gameObject.activeSelf) {
            tmpCombo.gameObject.SetActive(true);
        }
        if (!tmpJudgeResult.gameObject.activeSelf) {
            tmpJudgeResult.gameObject.SetActive(true);
        }


        tmpCombo.text = $"{combo} hits\nscore:{runningScore}";

        string judgeText;

        if ((judgeResult & Hit.PERFECT) != 0) {
            judgeText = "Perfect!";
        } else {
            judgeText = judgeResult switch {
                Hit.NO_HIT => "No Hit!",
                Hit.INCORRECT => "Wrong!",
                Hit.EARLY_MISS => "Miss! Too Early",
                Hit.EARLY_GREAT => "Great! Too Early",
                Hit.EARLY_GOOD => "Good! Too Early",
                Hit.LATE_MISS => "Miss! Too Late",
                Hit.LATE_GREAT => "Great! Too Late",
                Hit.LATE_GOOD => "Good! Too Late",
                _ => null
            };
        }

        tmpJudgeResult.text = judgeText;

        lastTriggerTime = Time.time;
    }

}
