using System;
using System.Collections.Generic;

using UnityEngine;

using Assets._Achromatic.Scripts.Beatmap;
using Assets._Achromatic.Scripts.Scores;
using Assets._Achromatic.Scripts.Pieces;

// Todo show score overlay UI

public class ScoreTracker {

    /// <summary>
    /// total score possible for a piece
    /// </summary>
    private const int TOTAL_SCORES = 10000;

    /// <summary>
    /// running score,
    /// maybe lower than actual due to integer
    /// </summary>
    public int runningScore;

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
    private readonly Dictionary<Hit, int> resultCnt;

    private readonly float perfectScore;
    private readonly float greatScore;
    private readonly float goodScore;
    private readonly int perfectScoreInt;
    private readonly int greatScoreInt;
    private readonly int goodScoreInt;


    public ScoreTracker(Notes notes) {
        // init resultCnt  -----------------------------------------------------
        resultCnt = new Dictionary<Hit, int>();
        foreach (Hit result
                in Enum.GetValues(typeof(Hit))) {
            resultCnt[result] = 0;  // filled w/ 0
        }


        // init perResultScores  -----------------------------------------------
        perfectScore = TOTAL_SCORES / notes.data.notes.Length;
        greatScore = perfectScore * 0.7f;
        goodScore = perfectScore * 0.3f;

        perfectScoreInt = (int)perfectScore;
        greatScoreInt = (int)greatScore;
        goodScoreInt = (int)goodScore;


        // init combo  ---------------------------------------------------------
        combo = 0;
        maxCombo = 0;
    }

    public void Record(Hit judgeResult) {
        // record the result as count
        resultCnt[judgeResult] += 1;

        // update running score
        if ((judgeResult & Hit.PERFECT) != 0) {
            runningScore += perfectScoreInt;
        } else if ((judgeResult & Hit.GREAT) != 0) {
            runningScore += greatScoreInt;
        } else if ((judgeResult & Hit.GOOD) != 0) {
            runningScore += goodScoreInt;
        }


        // record combo
        if ((judgeResult & Hit.NO_SCORE) != 0) {
            combo = 0;  // miss, reset combo
        } else {
            combo += 1;
            if (combo > maxCombo) {
                maxCombo = combo;
            }
        }

        Debug.Log("ScoreTracker:Record:"
                + $"\tjudge: {judgeResult}"
                + $"\tscore: {runningScore}"
                + $"\tcombo: {combo}"
                );
    }

    /// <returns>final correct/precise score</returns>
    public int CalcFinalScore() {
        float finalScore = 0.0f;

        foreach (Hit result
                in Enum.GetValues(typeof(Hit))) {

            if ((result & Hit.NO_SCORE) != 0) {
                continue;
            }

            if ((result & Hit.PERFECT) != 0) {
                finalScore += perfectScore;
            } else if ((result & Hit.GREAT) != 0) {
                finalScore += greatScore;
            } else {
                finalScore += goodScore;
            }
        }

        return Mathf.RoundToInt(finalScore);
    }
}

