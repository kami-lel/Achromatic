using UnityEngine;

public class TutorialSwitcher: MonoBehaviour {
    // Inspector Fields  #######################################################

    [SerializeField]
    private GameObject[] gamepadIcons;

    [SerializeField]
    private GameObject[] keyboardIcons;

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
        for (int i = 0; i < gamepadIcons.Length; i++) {
            gamepadIcons[i].SetActive(true);
        }

        for (int j = 0; j < keyboardIcons.Length; j++) {
            gamepadIcons[j].SetActive(false);
        }
    }

    private void OnSwitchToKeyboard() {
        for (int i = 0; i < gamepadIcons.Length; i++) {
            gamepadIcons[i].SetActive(false);
        }

        for (int j = 0; j < keyboardIcons.Length; j++) {
            gamepadIcons[j].SetActive(true);
        }
    }
}
