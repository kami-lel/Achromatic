using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreAdditionIndicatorScript: MonoBehaviour {

    // public method  ==========================================================


    // MonoBehavior Lifecycle  =================================================
    private void Awake() {
        textField = GetComponent<TextMeshProUGUI>();
    }

    public void Show(int scoreAddition) {
        textField.text = $"+{scoreAddition}";
        // todo animation for score addition
    }

    // private members  ========================================================
    private TextMeshProUGUI textField;

}
