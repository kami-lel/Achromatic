
using System;
using System.Collections.Generic;
using UnityEngine;

public class JudgeCriteria {
    // TODO refactor & documentation

    private Queue<Timing> timings;
    public EventHandler onDetectPassByMiss;

    public JudgeCriteria(BeatmapData beatmap) {
        // pre-calculate all judge timings
        timings = new();

        float beat0time = beatmap.PreludeLength;
        float secondPerBeat = 60.0f / beatmap.Tempo;

        foreach (BeatmapNote note in beatmap.notes) {
            // per note
            float centerTiming = secondPerBeat * note.CalcBeatCount()
                    + beat0time;
            timings.Enqueue(new Timing(centerTiming));
        }
    }

    public JudgeResult Judge(float time) {



        return JudgeResult.MISS;  // TODO
    }

    /// <summary>
    /// detect miss then player is too far away
    ///
    /// used in <c>Update()</c>
    /// </summary>
    public void DetectPassByMiss(float time) {
        if (timings.Count <= 0) {
            return;
        }

        while (timings.Peek().IsPassByMiss(time)) {
            // detect pass-by miss
            onDetectPassByMiss?.Invoke(this, EventArgs.Empty);

            timings.Dequeue();
            if (timings.Count <= 0) {
                break;
            }
        }
    }

    public class Timing {

        // todo dynamic time deltas
        private const float perfectDelta = 0.05f;
        private const float greatDelta = 0.10f;
        private const float goodDelta = 0.20f;

        private float leftGoodBound;
        private float leftGreatBound;
        private float leftPerfectBound;
        public float rightGoodBound; // HACK public
        private float rightGreatBound;
        private float rightPerfectBound;

        public Timing(float centerTiming) {
            leftGoodBound = centerTiming - goodDelta;
            leftGreatBound = centerTiming - greatDelta;
            leftPerfectBound = centerTiming - perfectDelta;
            rightGoodBound = centerTiming + goodDelta;
            rightGreatBound = centerTiming + greatDelta;
            rightPerfectBound = centerTiming + perfectDelta;
        }

        public JudgeResult Judge(float timing) {
            return JudgeResult.MISS;  // TODO
        }

        public bool IsPassByMiss(float time) {
            return time > rightGoodBound;
        }

        private bool IsInJudgingRange(float time) {
            return time < leftGoodBound;
        }
    }

}