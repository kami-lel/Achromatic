using System;
using System.Collections.Generic;

using UnityEngine;

using Assets._Achromatic.Scripts.Scores;

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
    private Dictionary<Judgement, int> resultCnt;

    private readonly float perfectScore;
    private readonly float greatScore;
    private readonly float goodScore;
    private readonly int perfectScoreInt;
    private readonly int greatScoreInt;
    private readonly int goodScoreInt;


    public ScoreTracker(BeatmapData beatmap) {
        // init resultCnt  -----------------------------------------------------
        resultCnt = new Dictionary<Judgement, int>();
        foreach (Judgement result
                in Enum.GetValues(typeof(Judgement))) {
            resultCnt[result] = 0;  // filled w/ 0
        }


        // init perResultScores  -----------------------------------------------
        perfectScore = TOTAL_SCORES / beatmap.notes.Count;
        greatScore = perfectScore * 0.7f;
        goodScore = perfectScore * 0.3f;

        perfectScoreInt = (int)perfectScore;
        greatScoreInt = (int)greatScore;
        goodScoreInt = (int)goodScore;


        // init combo  ---------------------------------------------------------
        combo = 0;
        maxCombo = 0;
    }

    public void Record(Judgement judgeResult) {
        // record the result as count
        resultCnt[judgeResult] += 1;

        // update running score
        if ((judgeResult & Judgement.PERFECT) != 0) {
            runningScore += perfectScoreInt;
        } else if ((judgeResult & Judgement.GREAT) != 0) {
            runningScore += greatScoreInt;
        } else if ((judgeResult & Judgement.GOOD) != 0) {
            runningScore += goodScoreInt;
        }


        // record combo
        if ((judgeResult & Judgement.NO_SCORE) != 0) {
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

        foreach (Judgement result
                in Enum.GetValues(typeof(Judgement))) {

            if ((result & Judgement.NO_SCORE) != 0) {
                continue;
            }

            if ((result & Judgement.PERFECT) != 0) {
                finalScore += perfectScore;
            } else if ((result & Judgement.GREAT) != 0) {
                finalScore += greatScore;
            } else {
                finalScore += goodScore;
            }
        }

        return Mathf.RoundToInt(finalScore);
    }
}

