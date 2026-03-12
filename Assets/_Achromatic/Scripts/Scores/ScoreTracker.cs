using System;
using System.Collections.Generic;

using UnityEngine;

using Assets._Achromatic.Scripts.Scores;
using Assets._Achromatic.Scripts.Pieces;
using Assets._Achromatic.Scripts.Beatmap;
using TMPro;
using UnityEngine.SceneManagement;

// todo improve looking of indicators

public class Score {

    // public members  =========================================================

    /// <summary>
    /// running score, maybe lower than actual points
    /// </summary>
    public float runningScore;

    /// <summary>
    /// number of current combos
    /// </summary>
    public int combo;

    /// <summary>
    /// number of max combos
    /// </summary>
    public int maxCombo;

    /// <summary>
    /// count each type of result
    /// </summary>
    public readonly Dictionary<Hit, int> hitCnt;

    // public methods  =========================================================

    public void Record(Hit hit) {
        // save results
        hitCnt[hit] += 1;

        // update running score
        if ((hit & Hit.PERFECT) != 0) {
            runningScore += perfectScore;
        } else if ((hit & Hit.GREAT) != 0) {
            runningScore += greatScore;
        } else if ((hit & Hit.GOOD) != 0) {
            runningScore += goodScore;
        }

        // record combo
        if ((hit & Hit.NO_SCORE) != 0) {
            combo = 0;  // miss, reset combo
        } else {
            combo += 1;
            if (combo > maxCombo) {
                maxCombo = combo;
            }
        }

        Debug.Log("Score.Record:"
                + $"\tjudge: {hit}"
                + $"\tscore: {runningScore}"
                + $"\tcombo: {combo}"
                );

        // update indicators  --------------------------------------------------
        // combo indicator
        if (comboIndicator != null) {
            comboIndicator.text = $"{combo}";
        }
        // running score
        if (runningScoreIndicator != null) {
            runningScoreIndicator.text = $"{(int)runningScore}";
        }
        // hit type indicator
        if (hitTypeIndicator != null) {
            hitTypeIndicator.Show(hit);
        }
    }


    // constructor  ============================================================
    public Score(Beatmap beatmap) {
        // init resultCnt  -----------------------------------------------------
        hitCnt = new Dictionary<Hit, int>();
        foreach (Hit result
                in Enum.GetValues(typeof(Hit))) {
            hitCnt[result] = 0;  // filled w/ 0
        }


        // init perResultScores  -----------------------------------------------
        perfectScore = TOTAL_SCORES / beatmap.data.notes.Length;
        greatScore = perfectScore * 0.7f;
        goodScore = perfectScore * 0.3f;


        // init combo  ---------------------------------------------------------
        combo = 0;
        maxCombo = 0;

        // init indicators
        InitIndicators();
        SceneManager.sceneLoaded += HandleInitIndicators;
    }

    // private members  ========================================================

    // cached references
    private TextMeshProUGUI comboIndicator;
    private TextMeshProUGUI runningScoreIndicator;
    private HitTypeIndicatorScript hitTypeIndicator;

    /// <summary>
    /// total score possible for a piece
    /// </summary>
    private const int TOTAL_SCORES = 10000;

    private readonly float perfectScore;
    private readonly float greatScore;
    private readonly float goodScore;

    // private methods  ========================================================
    private void InitIndicators() {
        // combo indicator
        GameObject comboGO = GameObject.FindWithTag("ComboIndicator");
        if (comboGO != null) {
            comboIndicator = comboGO.GetComponent<TextMeshProUGUI>();
        }
        if (comboIndicator == null) {
            Debug.LogError("Score: fail to find Combo Indicator");
        }

        // running score indicator
        GameObject runningGO = GameObject.FindWithTag("RunningScoreIndicator");
        if (comboGO != null) {
            runningScoreIndicator = runningGO.GetComponent<TextMeshProUGUI>();
        }
        if (runningScoreIndicator == null) {
            Debug.LogError("Score: fail to find Running Score Indicator");
        }

        // hit type indicator
        GameObject hitTypeGO = GameObject.FindWithTag("HitIndicator");
        if (hitTypeGO != null) {
            hitTypeIndicator = hitTypeGO.GetComponent<HitTypeIndicatorScript>();
        }
        if (hitTypeIndicator == null) {
            Debug.LogError("Score: fail to find Hit Type Indicator");
        }
    }

    private void HandleInitIndicators(Scene scene, LoadSceneMode mode) {
        InitIndicators();
    }
}

