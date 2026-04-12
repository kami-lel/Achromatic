using TMPro;
using UnityEngine;

public class FPSCounter: MonoBehaviour {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    // Public Members  #########################################################

    // singleton
    public static FPSCounter I {
        get; private set;
    }

    public int lastFPS;

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        I = this;

        GameObject go = GameObject.FindWithTag(
            FPS_COUNTER_TAG
        );

        if (go != null) {
            fpsCounterTextField = go.GetComponent<TextMeshProUGUI>();
        }

        if (fpsCounterTextField == null) {
            Debug.LogWarning("fail to find: FPS Counter text field", this);
        }
    }

    private void Update() {
        if (fpsCounterTextField == null) {
            return;
        }

        frameCounter++;
        fpsCounterAccumulateTime += Time.unscaledDeltaTime;
        if (fpsCounterAccumulateTime > 1.0f) {
            fpsCounterTextField.text = frameCounter + " fps";
            lastFPS = frameCounter;
            frameCounter = 0;
            fpsCounterAccumulateTime = 0.0f;
        }
    }

    // constants  ##############################################################
    private const string FPS_COUNTER_TAG = "FPSCounter";

    // private members  ########################################################

    private TextMeshProUGUI fpsCounterTextField;
    private int frameCounter = 0;
    private float fpsCounterAccumulateTime = 0.0f;

#endif
}
