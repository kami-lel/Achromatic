using Assets._Achromatic.Scripts.Scores;
using TMPro;
using UnityEngine;


// HACK rm tags

public class RunningScoreIndicator: MonoBehaviour {


    // Inspector Fields  #######################################################

    [SerializeField]
    private Score score;

    [Header("Internals")]

    [SerializeField]
    private TextMeshProUGUI comboIndicator;

    [SerializeField]
    private TextMeshProUGUI runningScoreIndicator;

    [SerializeField]
    private TextMeshProUGUI scoreAdditionIndicator;

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        // Inspector Assignment Guard ------------------------------------------
        if (score == null) {
            Debug.LogWarning("must assign: score", this);
        }
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

    private void OnEnable() {
        score.OnScoreChange += UpdateScore;
    }

    private void OnDisable() {
        score.OnScoreChange -= UpdateScore;
    }

    // private methods  ########################################################

    public void UpdateScore(int combo, int runningScore, int scoreAddition) {
        // todo animation
        comboIndicator.text = $"{combo}";
        runningScoreIndicator.text = $"{runningScore}";
        scoreAdditionIndicator.text = $"+{scoreAddition}";
    }

}


