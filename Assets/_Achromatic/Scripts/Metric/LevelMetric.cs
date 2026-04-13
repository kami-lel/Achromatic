#if UNITY_EDITOR || DEVELOPMENT_BUILD

using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Achromatic.Scripts.Scores;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;

namespace Assets._Achromatic.Scripts.Metric {
    [Serializable]
    public class LevelMetric {

        // Public Members  #####################################################
        public List<int> fps;

        public int fpsMax = -1;
        public int fpsMin = -1;
        public float fpsMean = -1f;

        public float totalScore = -1f;
        public int maxCombo = -1;

        [Serializable]
        public class Timing {
            public float levelStart = -1f;
            public float musicStart = -1f;
            public float musicEnd = -1f;
            public float windowClose = -1f;
            public float levelEnd = -1f;
        }

        public Timing timings;

        [Serializable]
        public class Interval {
            public float startExplore = -1f;
            public float musicPlay = -1f;
            public float finalPointWindow = -1f;
            public float endExplore = -1f;
            public float total = -1f;
        }

        public Interval intervals;

        [Serializable]
        public class Percentage {
            public float startExplore = -1f;
            public float musicPlay = -1f;
            public float finalPointWindow = -1f;
            public float endExplore = -1f;
        }

        public Percentage percentages;


        [Serializable]
        public class HitsCount {
            public int perfect;
            public int great;
            public int good;

            public HitsCount(Score score) {

                // TODO all hits & deltas
            }

        }

        public HitsCount hitsCount;

        // Public Methods  #####################################################

        public void LogScore(Score score) {
            totalScore = score.runningScore;
            maxCombo = score.maxCombo;

            // TODO metrics: hit / miss ratio per part
        }

        public void FinishLevel() {
            // FPS  ------------------------------------------------------------
            if (fps.Count > 0) {
                fpsMin = fps.Min();
                fpsMax = fps.Max();

                float total = 0f;
                for (int i = 0; i < fps.Count; i++) {
                    total += fps[i];
                }
                fpsMean = (float)total / fps.Count;
            }

            // timing & interval  ----------------------------------------------
            // interval
            intervals.startExplore = CalcInterval(
                timings.levelStart,
                timings.musicStart
            );
            intervals.musicPlay = CalcInterval(
                timings.musicStart,
                timings.musicEnd
            );
            intervals.finalPointWindow = CalcInterval(
                timings.musicEnd,
                timings.windowClose
            );
            intervals.endExplore = CalcInterval(
                timings.windowClose,
                timings.levelEnd
            );
            intervals.total = CalcInterval(
                timings.levelStart,
                timings.levelEnd
            );

            // percentage
            percentages.startExplore = CalcPercentage(intervals.startExplore);
            percentages.musicPlay = CalcPercentage(intervals.musicPlay);
            percentages.finalPointWindow = CalcPercentage(
                intervals.finalPointWindow
            );
            percentages.endExplore = CalcPercentage(intervals.endExplore);
        }

        // Constructor  ########################################################
        public LevelMetric() {
            fps = new List<int>();

            timings = new Timing();
            intervals = new Interval();
            percentages = new Percentage();
        }

        // private methods  ####################################################

        private static float CalcInterval(float from, float to) {
            if (from == -1f || to == -1f) {
                return -1f;
            }

            float value = to - from;
            return value < 0 ? -1f : value;
        }

        private float CalcPercentage(float intervalValue) {
            if (intervals.total == -1f || intervalValue == -1f) {
                return -1f;
            }

            return intervalValue / intervals.total;
        }
    }
}

#endif
