
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
        if (timings.Count <= 0) {
            return JudgeResult.NO_HIT;
        }

        Timing timing = timings.Peek();
        if (timing.IsInJudgingRange(time)) {
            timing = timings.Dequeue();
            return timing.Judge(time);

        } else {
            return JudgeResult.NO_HIT;
        }
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

        private readonly float leftGoodBound;
        private readonly float leftGreatBound;
        private readonly float leftPerfectBound;
        private readonly float rightGoodBound;
        private readonly float rightGreatBound;
        private readonly float rightPerfectBound;

        public Timing(float centerTiming) {
            leftGoodBound = centerTiming - goodDelta;
            leftGreatBound = centerTiming - greatDelta;
            leftPerfectBound = centerTiming - perfectDelta;
            rightGoodBound = centerTiming + goodDelta;
            rightGreatBound = centerTiming + greatDelta;
            rightPerfectBound = centerTiming + perfectDelta;
        }

        public JudgeResult Judge(float time) {
            if (leftPerfectBound < time && time < rightPerfectBound) {
                return JudgeResult.PERFECT;
            } else if (leftGreatBound < time && time < rightGreatBound) {
                return JudgeResult.GREAT;
            } else if (leftGoodBound < time && time < rightGoodBound) {
                return JudgeResult.GOOD;
            } else {
                return JudgeResult.MISS;
            }
        }

        public bool IsPassByMiss(float time) {
            return time > rightGoodBound;
        }

        public bool IsInJudgingRange(float time) {
            return time < leftGoodBound;
        }
    }

}