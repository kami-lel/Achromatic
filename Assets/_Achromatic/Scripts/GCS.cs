using System;
using Assets._Achromatic.Scripts.Scores;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

// TODO metrics: fps
// TODO metrics: total time &portion of time
// TODO frame counter
// Todo metrics: deltas
// Todo metrics: hit / miss ratio per part
// Todo merge game stat

public class GCS: MonoBehaviour {

    // singleton
    public static GCS I {
        get; private set;
    }

    public GameState states;

    // MonoBehavior Lifecycle  #################################################
    private void Awake() { // ==================================================

        // ensure Singleton  ---------------------------------------------------
        if (I == null) {
            I = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        if (I != this) {  // guard against duplicate
            Debug.LogError("GameController:\tplace GameController Prefab only in 1st scene");
            Destroy(gameObject);
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        InitFPSCounter();
        SceneManager.sceneLoaded += HandleInitFPSCounter;
#endif

        // Hack rm these
        // reference to playerScript
        playerScript = player.GetComponent<PlayerScript>();


        // disable textbox
        tmpCombo.gameObject.SetActive(false);
        tmpJudgeResult.gameObject.SetActive(false);

    }

    private void Update() {  // ================================================
        float scale = tmpTextboxCurve.Evaluate(Time.time - lastTriggerTime);
        // Hack
        // tmpJudgeResult.transform.localScale = new Vector3(scale, scale);

    }


    // Fixme create score overlay

    // Inspector Fields  #######################################################

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private bool enablesFPSCounter = true;

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
    public static GameObject GetPlayer() { // Hack rm
        if (I == null) {
            Debug.LogError("GameController:\tInstance is null");
        }

        return I.player;
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


    // private members  ########################################################
    // cached references
    private TMP_Text fpsCounter;


    // private methods  ########################################################

#if UNITY_EDITOR || DEVELOPMENT_BUILD

    private void InitFPSCounter() {
        var fpsCounterGameObject = GameObject.FindWithTag("FPSCounter");
        if (fpsCounterGameObject != null) {
            fpsCounter = fpsCounterGameObject.GetComponent<TMP_Text>();
        }

        if (fpsCounter != null) {
            fpsCounterGameObject.SetActive(true);
            fpsCounter.text = "???";  // HACK
        } else {
            Debug.LogWarning("GCS: fail to find FPS Counter text field");
        }
    }

    private void HandleInitFPSCounter(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode) {
        InitFPSCounter();
    }

#endif

}
