using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger: MonoBehaviour {

    // Public Members  #########################################################

    // Singleton
    public static SceneChanger I;

    // Public Methods  #########################################################

    public void LoadNextScene(string sceneName) {
        Debug.Log("LoadNextScene", this);

        // Todo change game state

        CinemachineVirtualCamera ending =
                FindVirtualCameraByTag(END_CAMERA_TAG);
        ending.Priority = HIGH_CAMERA_PRIORITY;


        fadingBlockingPanel.FadeOut(
                onComplete: () => StartCoroutine(LoadSceneCoroutine(sceneName))
        );
    }

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // singleton logic  ----------------------------------------------------
        if (I != null && I != this) {
            Debug.LogWarning("duplicated SceneManager", this);
            Destroy(gameObject);
        } else {
            I = this;
            DontDestroyOnLoad(gameObject);
        }

        // event manager  ------------------------------------------------------
        SceneManager.sceneLoaded += OnSceneLoaded;

        // find fading panel  --------------------------------------------------
        GameObject go;
        go = GameObject.FindGameObjectWithTag(BLOCKING_PANEL_TAG);
        if (go == null) {
            Debug.LogError("fail to find Fading Blocking Panel");
        }
        fadingBlockingPanel = go.GetComponent<FadingBlockingPanel>();
        if (fadingBlockingPanel == null) {
            Debug.LogError("fail to find Fading Blocking Panel");
        }
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Event Handler  ##########################################################

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        EnterNewScene();
    }

    // constants  ##############################################################

    private const int HIGH_CAMERA_PRIORITY = 100;
    private const int LOW_CAMERA_PRIORITY = 0;
    private const string START_CAMERA_TAG = "startingCamera";
    private const string END_CAMERA_TAG = "endingCamera";
    private const string BLOCKING_PANEL_TAG = "fadingBlockingPanel";


    // private members  ########################################################

    private FadingBlockingPanel fadingBlockingPanel;

    // private methods  ########################################################

    private void EnterNewScene() {
        Debug.Log("EnterNewScene", this);

        CinemachineVirtualCamera startingCamera =
                FindVirtualCameraByTag(START_CAMERA_TAG);
        startingCamera.Priority = HIGH_CAMERA_PRIORITY;

        fadingBlockingPanel.FadeIn(onComplete: () => {
            startingCamera.Priority = LOW_CAMERA_PRIORITY;
        });

        GCS.I.states = GameState.EXPLORE;
    }


    // helpers  ================================================================

    private CinemachineVirtualCamera FindVirtualCameraByTag(string tag) {
        GameObject go;
        go = GameObject.FindGameObjectWithTag(tag);
        if (go == null) {
            Debug.LogError(
                $"fail to find Virtual Camera with tag: {tag}"
            );
            return null;
        }

        var virtualCamera = go.GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera == null) {
            Debug.LogError(
                $"GameObject does not have Virtual Camera Component"
            );
        }

        return virtualCamera;
    }

    private IEnumerator LoadSceneCoroutine(string sceneName) {
        AsyncOperation op =
            SceneManager.LoadSceneAsync(sceneName);

        if (op == null) {
            Debug.LogError($"fail to load scene: {sceneName}", this);
            yield break;
        }

        op.allowSceneActivation = false;

        // wait for load to reach 90% (Unity max before activation)
        while (op.progress < 0.9f)
            yield return null;

        op.allowSceneActivation = true;  // triggers OnSceneLoaded
    }

}

