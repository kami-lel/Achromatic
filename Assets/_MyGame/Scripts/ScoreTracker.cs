

using System;
using System.Collections.Generic;
using UnityEngine;


// Todo docs
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

    public int combo;

    private Dictionary<JudgeResult, int> resultCnt;
    private readonly float[] perResultScores;
    private readonly int[] perResultScoresInt;

    public ScoreTracker(BeatmapData beatmap) {
        // init resultCnt  -----------------------------------------------------
        resultCnt = new Dictionary<JudgeResult, int>();
        foreach (JudgeResult result
                in Enum.GetValues(typeof(JudgeResult))) {
            resultCnt[result] = 0;  // filled w/ 0
        }

        // init perResultScores  -----------------------------------------------
        float perfectScore = TOTAL_SCORES / beatmap.notes.Count;
        float greatScore = perfectScore * 0.7f;
        float goodScore = perfectScore * 0.3f;

        int enumTypeCount = Enum.GetValues(typeof(JudgeResult)).Length;
        perResultScores = new float[enumTypeCount];
        perResultScores[(int)JudgeResult.PERFECT] = perfectScore;
        perResultScores[(int)JudgeResult.GREAT] = greatScore;
        perResultScores[(int)JudgeResult.GOOD] = goodScore;
        perResultScores[(int)JudgeResult.MISS] = 0.0f;

        // init perResultScoresInt ---------------------------------------------
        perResultScoresInt = new int[enumTypeCount];
        for (int i = 0; i < enumTypeCount; i++) {
            perResultScoresInt[i] = (int)perResultScores[i];
        }

        // init combo  ---------------------------------------------------------
        combo = 0;
    }

    public void Record(JudgeResult judgeResult) {
        // record the result
        resultCnt[judgeResult] += 1;

        // update running score
        runningScore += perResultScoresInt[(int)judgeResult];

        // record combo
        if (judgeResult == JudgeResult.MISS) {
            combo = 0;
        } else {
            combo += 1;
        }
    }

    public int CalcFinalScore() {
        float finalScore = 0.0f;

        foreach (JudgeResult result
                in Enum.GetValues(typeof(JudgeResult))) {
            float score = perResultScores[(int)result] * resultCnt[result];
            finalScore += score;
        }

        return Mathf.RoundToInt(finalScore);
    }
}


public enum JudgeResult {
    MISS = 0, GOOD = 1, GREAT = 2, PERFECT = 3
}