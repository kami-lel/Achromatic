using UnityEngine;

public class TutorialSwitcher: MonoBehaviour {
    // Inspector Fields  #######################################################

    [SerializeField]
    private Transform icons;

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
        SetChildrenActive(true);
    }

    private void OnSwitchToKeyboard() {
        SetChildrenActive(false);
    }

    // constants  ##############################################################

    private const string GAMEPAD_TAG = "TutorialGamepad";
    private const string KEYBOARD_TAG = "TutorialKeyboard";

    // private methods  ########################################################

    private void SetChildrenActive(bool isGamePad) {
        int count = icons.childCount;

        for (int i = 0; i < count; i++) {
            Transform level1 = icons.GetChild(i);

            int level1Count = level1.childCount;

            for (int j = 0; j < level1Count; j++) {
                Transform level2 = level1.GetChild(j);

                if (level2.CompareTag(GAMEPAD_TAG)) {
                    level2.gameObject.SetActive(isGamePad);
                } else if (level2.CompareTag(KEYBOARD_TAG)) {
                    level2.gameObject.SetActive(!isGamePad);
                }
            }
        }
    }
}
