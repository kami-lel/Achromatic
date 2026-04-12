using TMPro;
using UnityEngine;

public class FPSCounter: MonoBehaviour {
#if UNITY_EDITOR || DEVELOPMENT_BUILD

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
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

        fpsFrameCounter++;
        fpsCounterAccumulateTime += Time.unscaledDeltaTime;
        if (fpsCounterAccumulateTime > 1.0f) {
            fpsCounterTextField.text = fpsFrameCounter + " fps";
            fpsFrameCounter = 0;
            fpsCounterAccumulateTime = 0.0f;
        }
    }

    // constants  ##############################################################
    private const string FPS_COUNTER_TAG = "FPSCounter";

    // private members  ########################################################

    private TextMeshProUGUI fpsCounterTextField;
    private int fpsFrameCounter = 0;
    private float fpsCounterAccumulateTime = 0.0f;

#endif
}
