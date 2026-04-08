using System;

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using UnityEngine.Profiling;
using Unity.VectorGraphics;

[RequireComponent(typeof(SceneChanger))]

// Todo metrics: fps,total time &portion of time,deltas,hit / miss ratio per part
// Fixme merge game stat

public class GCS: MonoBehaviour {

    // Public Members  #########################################################

    // singleton
    public static GCS I {
        get; private set;
    }

    [NonSerialized]
    public GameState states = GameState.NONE;

    // Public Methods  #########################################################

    public static GameObject FindPlayer() {
        GameObject playerObject = GameObject.FindWithTag(PLAYER_TAG);

        if (playerObject == null) {
            Debug.LogError($"GCS:\tfail to find GameObject with tag: {PLAYER_TAG}");
        }

        return playerObject;
    }

    public void LoadNextScene(string sceneName) {
        sceneChanger.LoadNextScene(sceneName);
    }

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // ensure Singleton  ---------------------------------------------------
        if (I == null) {
            I = this;
            DontDestroyOnLoad(gameObject);
        } else if (I != this) {  // guard against duplicate
            Debug.LogError("GameController:\tplace GameController Prefab only in 1st scene", this);
            Destroy(gameObject);
            return;
        }

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
        return;  // Hack
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

    // constants  ##############################################################
    private const string PLAYER_TAG = "Player";

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
