using Assets._Achromatic.Scripts.Scores;
using TMPro;
using UnityEngine;


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

    private void Update() {
        if (GameController.I.states == GameState.TOTAL_SCORE_WINDOW) {
            // hack better way to do this
            comboIndicator.text = "";
            runningScoreIndicator.text = "";
            scoreAdditionIndicator.text = "";
        }

    }

    private void OnEnable() {
        score.OnScoreChange += UpdateScore;
    }

    private void OnDisable() {
        score.OnScoreChange -= UpdateScore;
    }

    // private methods  ########################################################

    private void UpdateScore(int combo, int runningScore, int scoreAddition) {
        // todo animation
        comboIndicator.text = $"{combo}";
        runningScoreIndicator.text = $"{runningScore}";
        scoreAdditionIndicator.text = $"+{scoreAddition}";
    }

}


