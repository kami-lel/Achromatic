using UnityEngine;

public class TutorialSwitcher: MonoBehaviour {
    // Inspector Fields  #######################################################

    [SerializeField]
    private Transform gamepadIcons;

    [SerializeField]
    private Transform keyboardIcons;

    // MonoBehavior Lifecycle  #################################################

    private void OnEnable() {
        GamepadManager.I.OnSwitchToGamepad += OnSwitchToGamepad;
        GamepadManager.I.OnSwitchToKeyboard += OnSwitchToKeyboard;
    }

    private void OnDisable() {
        GamepadManager.I.OnSwitchToGamepad -= OnSwitchToGamepad;
        GamepadManager.I.OnSwitchToKeyboard -= OnSwitchToKeyboard;
    }

    // Event Handler  ##########################################################

    private void OnSwitchToGamepad() {
        ChildrenSetActive(gamepadIcons, true);
        ChildrenSetActive(keyboardIcons, false);
    }

    private void OnSwitchToKeyboard() {
        ChildrenSetActive(gamepadIcons, false);
        ChildrenSetActive(keyboardIcons, true);
    }

    // private methods  ########################################################

    private void ChildrenSetActive(Transform transform, bool isActive) {
        int count = transform.childCount;

        for (int i = 0; i < count; i++) {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(isActive);
        }

    }
}
