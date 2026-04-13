using UnityEngine;

public class StarterTutorial: MonoBehaviour {

    // Inspector Fields  #######################################################

    [SerializeField]
    private Transform gamepadIcon;

    [SerializeField]
    private Transform keyboardIcon;

    [SerializeField]
    private float smallIconScaling = 0.8f;

    [SerializeField]
    private float largerIconScaling = 2.0f;

    [SerializeField]
    private float scaleTweenDuration = 0.25f;

    // MonoBehavior Lifecycle  #################################################

    private Vector3 _gamepadBaseScale;
    private Vector3 _keyboardBaseScale;

    private Coroutine _scaleRoutine;

    private void Awake() {
        _gamepadBaseScale = gamepadIcon.localScale;
        _keyboardBaseScale = keyboardIcon.localScale;
    }

    private void OnEnable() {
        GamepadManager.I.OnSwitchToGamepad += OnSwitchToGamepad;
        GamepadManager.I.OnSwitchToKeyboard += OnSwitchToKeyboard;
    }

    private void OnDisable() {
        GamepadManager.I.OnSwitchToGamepad -= OnSwitchToGamepad;
        GamepadManager.I.OnSwitchToKeyboard -= OnSwitchToKeyboard;
        if (_scaleRoutine != null) {
            StopCoroutine(_scaleRoutine);
            _scaleRoutine = null;
        }
    }

    // Event Handler  ##########################################################

    private void OnSwitchToGamepad() {
        StartScaleTo(largerIconScaling, smallIconScaling);
    }

    private void OnSwitchToKeyboard() {
        StartScaleTo(smallIconScaling, largerIconScaling);
    }


    // private methods  ########################################################

    private void StartScaleTo(float gamepadMul, float keyboardMul) {
        if (_scaleRoutine != null) {
            StopCoroutine(_scaleRoutine);
            _scaleRoutine = null;
        }

        Vector3 toGamepadScale = _gamepadBaseScale * gamepadMul;
        Vector3 toKeyboardScale = _keyboardBaseScale * keyboardMul;

        _scaleRoutine = StartCoroutine(
            ScaleBothIconsRoutine(gamepadIcon, toGamepadScale,
            keyboardIcon, toKeyboardScale, scaleTweenDuration)
        );
    }

    private System.Collections.IEnumerator ScaleBothIconsRoutine(
        Transform gp,
        Vector3 toGp,
        Transform kb,
        Vector3 toKb,
        float duration
    ) {
        Vector3 fromGp = gp.localScale;
        Vector3 fromKb = kb.localScale;

        float t = 0f;

        while (t < duration) {
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / duration);

            float eased = Mathf.SmoothStep(0f, 1f, u);

            gp.localScale = Vector3.LerpUnclamped(fromGp, toGp, eased);
            kb.localScale = Vector3.LerpUnclamped(fromKb, toKb, eased);

            yield return null;
        }

        gp.localScale = toGp;
        kb.localScale = toKb;
        _scaleRoutine = null;
    }

}