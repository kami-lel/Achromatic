
using System.Collections.Generic;

public class JudgeCriteria {

    private Queue<float[]> timings;

    public JudgeCriteria(PieceScript script) {
        // pre-calculate all judge timings
        // TODO
    }

    public JudgeResult Judge(float time) {
        return JudgeResult.MISS;  // TODO
    }

    /// <summary>
    /// detect miss then player is too far away
    ///
    /// used in <c>Update()</c>
    /// </summary>
    public void DetectMiss() {
        // TODO TODO
    }
}