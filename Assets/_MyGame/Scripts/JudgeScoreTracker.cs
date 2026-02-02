

using System;
using System.Collections.Generic;

public class JudgeScoreTracker {

    private const int TOTAL_SCORES = 1000;

    /// <summary>
    /// running score,
    /// maybe lower than actual due to integer
    /// </summary>
    public int runningScore;

    private Dictionary<JudgeResult, int> resultCnt;
    private int[] perResultScores;

    public JudgeScoreTracker(BeatmapData beatmap) {
        // init resultCnt  -----------------------------------------------------
        resultCnt = new Dictionary<JudgeResult, int>();
        foreach (JudgeResult result
                in Enum.GetValues(typeof(JudgeResult))) {
            resultCnt[result] = 0;  // filled w/ 0
        }

        // init perResultScores  -----------------------------------------------
        perResultScores = new int[Enum.GetValues(typeof(JudgeResult)).Length];
        float perfectScore = TOTAL_SCORES / (beatmap.notes.Count);
    }

    public void Record(JudgeResult judgeResult) {
        resultCnt[judgeResult] += 1;
    }


}


public enum JudgeResult {
    MISS = 0, GOOD = 1, GREAT = 2, PERFECT = 3
}