// HACK rm

using System;

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using UnityEngine.Profiling;
using Unity.VectorGraphics;


// TODO metrics: fps,total time &portion of time,deltas,hit / miss ratio per part
// FIXME merge game stat

[RequireComponent(typeof(SceneChanger))]
[RequireComponent(typeof(GameController))]
public class GCS: MonoBehaviour {

    // Public Members  #########################################################

    // singleton
    public static GCS I {
        get; private set;
    }

    [NonSerialized]
    public GameState states = GameState.NONE;

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // singleton logic  ----------------------------------------------------
        I = this;

        // caching references to piece  ----------------------------------------
        sceneChanger = GetComponent<SceneChanger>();
        if (sceneChanger == null) {
            Debug.LogError("fail to get: SceneChanger", this);
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        SceneManager.sceneLoaded += HandleInitFPSCounter;
#endif
    }

    private void Update() {
        return;  // HACK rm or fix GCS for FPS counter etc.
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

    // private members  ########################################################
    // cached references
    private SceneChanger sceneChanger;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private TextMeshProUGUI fpsCounter;
    private int fpsFrameCounter = 0;
    private float fpsCounterAccumulateTime = 0.0f;
#endif

    // private methods  ########################################################
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    private void HandleInitFPSCounter(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode) {
        GameObject fpsCounterGameObject = GameObject.FindWithTag("FPSCounter");
        if (fpsCounterGameObject != null) {
            fpsCounter = fpsCounterGameObject.GetComponent<TextMeshProUGUI>();
        }
        if (fpsCounter == null) {
            Debug.LogWarning("fail to find: FPS Counter text field", this);
        }
    }
#endif

}
