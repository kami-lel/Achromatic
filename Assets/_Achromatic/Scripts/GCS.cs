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


    // public member  ##########################################################
    [NonSerialized]
    public GameState states = GameState.NONE;

    // MonoBehavior Lifecycle  #################################################
    private void Awake() { // ==================================================
        // ensure Singleton  ---------------------------------------------------
        if (I == null) {
            I = this;
            DontDestroyOnLoad(gameObject);
        } else if (I != this) {  // guard against duplicate
            Debug.LogError("GameController:\tplace GameController Prefab only in 1st scene");
            Destroy(gameObject);
            return;
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        SceneManager.sceneLoaded += HandleInitFPSCounter;
#endif

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
    private TMPro.TextMeshProUGUI tmpJudgeResult;

    [SerializeField]
    private TMPro.TextMeshProUGUI tmpCombo;

    [SerializeField]
    private AnimationCurve tmpTextboxCurve;

    // public members  #########################################################

    private float lastTriggerTime;

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
    private TextMeshProUGUI fpsCounter;

    // private methods  ########################################################

#if UNITY_EDITOR || DEVELOPMENT_BUILD

    private void HandleInitFPSCounter(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode) {
        GameObject fpsCounterGameObject = GameObject.FindWithTag("FPSCounter");
        if (fpsCounterGameObject != null) {
            fpsCounter = fpsCounterGameObject.GetComponent<TextMeshProUGUI>();
        }

        if (fpsCounter != null) {
            fpsCounterGameObject.SetActive(true);
        } else {
            Debug.LogWarning("GCS: fail to find FPS Counter text field");
        }
    }

#endif

}
