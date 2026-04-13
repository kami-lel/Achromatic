using TMPro;
using UnityEngine;


// HACK rm tags

public class RunningScoreIndicator: MonoBehaviour {


    // Public Methods  #########################################################

    public void UpdateScore(int combo, int runningScore, int scoreAddition) {
        // todo animation
        comboIndicator.text = $"{combo}";
        runningScoreIndicator.text = $"{runningScore}";
        scoreAdditionIndicator.text = $"+{scoreAddition}";
    }

    // Inspector Fields  #######################################################

    [SerializeField]
    private TextMeshProUGUI comboIndicator;

    [SerializeField]
    private TextMeshProUGUI runningScoreIndicator;

    [SerializeField]
    private TextMeshProUGUI scoreAdditionIndicator;

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // Inspector Assignment Guard ------------------------------------------
        if (comboIndicator == null) {
            Debug.LogWarning("must assign: comboIndicator", this);
        }
        if (runningScoreIndicator == null) {
            Debug.LogWarning("must assign: runningScoreIndicator", this);
        }
        if (scoreAdditionIndicator == null) {
            Debug.LogWarning("must assign: scoreAdditionIndicator", this);
        }
    }
}

