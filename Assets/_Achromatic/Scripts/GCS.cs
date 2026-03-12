using System;

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using Assets._Achromatic.Scripts.Scores;
using UnityEngine.Profiling;

// todo metrics: fps
// todo metrics: total time &portion of time
// todo metrics: deltas
// todo metrics: hit / miss ratio per part
// todo merge game stat

public class GCS: MonoBehaviour {

    // singleton
    public static GCS I {
        get; private set;
    }

    // public member  ==========================================================
    [NonSerialized]
    public GameState states = GameState.NONE;


    // MonoBehavior Lifecycle  =================================================

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
    }

    private void Update() {  // ================================================

        // FPS Counter  --------------------------------------------------------
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        fpsFrameCounter++;
        fpsCounterAccumulateTime += Time.unscaledDeltaTime;
        if (fpsCounterAccumulateTime > 1.0f) {
            fpsCounter.text = fpsFrameCounter + " fps";

            Profiler.BeginSample($"{fpsFrameCounter}");
            Profiler.EndSample();

            fpsFrameCounter = 0;
            fpsCounterAccumulateTime = 0.0f;
        }
#endif

    }

    // private members  ========================================================
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private TextMeshProUGUI fpsCounter;
    private int fpsFrameCounter = 0;
    private float fpsCounterAccumulateTime = 0.0f;
#endif

    // private methods  ========================================================

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private void HandleInitFPSCounter(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode) {
        GameObject fpsCounterGameObject = GameObject.FindWithTag("FPSCounter");
        if (fpsCounterGameObject != null) {
            fpsCounter = fpsCounterGameObject.GetComponent<TextMeshProUGUI>();
        }
        if (fpsCounter == null) {
            Debug.LogWarning("GCS: fail to find FPS Counter text field");
        }
    }
#endif

}
