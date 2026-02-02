
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            // TODO judge note type!
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
        private const float goodDelta = 0.30f;

        private readonly float startJudgeBound;
        private readonly float center;

        private readonly float leftGoodBound;
        private readonly float leftGreatBound;
        private readonly float leftPerfectBound;
        private readonly float rightGoodBound;
        private readonly float rightGreatBound;
        private readonly float rightPerfectBound;

        public Timing(float centerTiming) {
            center = centerTiming;
            leftGoodBound = centerTiming - goodDelta;
            // fixme better generation
            startJudgeBound = leftGoodBound - goodDelta;
            leftGreatBound = centerTiming - greatDelta;
            leftPerfectBound = centerTiming - perfectDelta;
            rightGoodBound = centerTiming + goodDelta;
            rightGreatBound = centerTiming + greatDelta;
            rightPerfectBound = centerTiming + perfectDelta;
        }

        public JudgeResult Judge(float time) {
            if (time < leftGoodBound) {
                return JudgeResult.EARLY_MISS;
            } else if (time < leftGreatBound) {
                return JudgeResult.EARLY_GOOD;
            } else if (time < leftPerfectBound) {
                return JudgeResult.EARLY_GREAT;
            } else if (time < center) {
                return JudgeResult.EARLY_PERFECT;
            } else if (time < rightPerfectBound) {
                return JudgeResult.LATE_PERFECT;
            } else if (time < rightGreatBound) {
                return JudgeResult.LATE_GREAT;
            } else if (time < rightGoodBound) {
                return JudgeResult.LATE_GOOD;
            } else {
                return JudgeResult.LATE_MISS;
            }
        }

        public bool IsPassByMiss(float time) {
            return time > rightGoodBound;
        }

        public bool IsInJudgingRange(float time) {
            return time > startJudgeBound;
        }
    }

}