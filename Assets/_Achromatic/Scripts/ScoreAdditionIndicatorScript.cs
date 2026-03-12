using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreAdditionIndicatorScript : MonoBehaviour
{

    // public method  ==========================================================

    public void Show(int scoreAddition)
    {
        textField.text = $"+{scoreAddition}";
        // Todo animation for score addition
    }

    // MonoBehavior Lifecycle  =================================================
    private void Awake()
    {
        textField = GetComponent<TextMeshProUGUI>();
    }

    // private members  ========================================================
    private TextMeshProUGUI textField;

}
