using System;
using System.Collections.Generic;
using UnityEngine;


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
    private Dictionary<JudgeResult, int> resultCnt;

    private readonly float perfectScore;
    private readonly float greatScore;
    private readonly float goodScore;
    private readonly int perfectScoreInt;
    private readonly int greatScoreInt;
    private readonly int goodScoreInt;


    public ScoreTracker(BeatmapData beatmap) {
        // init resultCnt  -----------------------------------------------------
        resultCnt = new Dictionary<JudgeResult, int>();
        foreach (JudgeResult result
                in Enum.GetValues(typeof(JudgeResult))) {
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

    public void Record(JudgeResult judgeResult) {
        // record the result as count
        resultCnt[judgeResult] += 1;

        // update running score
        if ((judgeResult & JudgeResult.PERFECT) != 0) {
            runningScore += perfectScoreInt;
        } else if ((judgeResult & JudgeResult.GREAT) != 0) {
            runningScore += greatScoreInt;
        } else if ((judgeResult & JudgeResult.GOOD) != 0) {
            runningScore += goodScoreInt;
        }


        // record combo
        if ((judgeResult & JudgeResult.NO_SCORE) != 0) {
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

        foreach (JudgeResult result
                in Enum.GetValues(typeof(JudgeResult))) {

            if ((result & JudgeResult.NO_SCORE) != 0) {
                continue;
            }

            if ((result & JudgeResult.PERFECT) != 0) {
                finalScore += perfectScore;
            } else if ((result & JudgeResult.GREAT) != 0) {
                finalScore += greatScore;
            } else {
                finalScore += goodScore;
            }
        }

        return Mathf.RoundToInt(finalScore);
    }
}

