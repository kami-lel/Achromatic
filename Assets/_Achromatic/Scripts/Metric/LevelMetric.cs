#if UNITY_EDITOR || DEVELOPMENT_BUILD

using System;
using System.Collections.Generic;
using System.Linq;

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

        public float timingLevelStart = -1f;
        public float timingMusicStart = -1f;
        public float timingMusicEnd = -1f;
        public float timingWindowClose = -1f;
        public float timingLevelEnd = -1f;

        public float intervalStartExplore = -1f;
        public float intervalMusicPlay = -1f;
        public float intervalFinalPointWindow = -1f;
        public float intervalEndExplore = -1f;
        public float intervalTotal = -1f;

        public float percentageStartExplore = -1f;
        public float percentageMusicPlay = -1f;
        public float percentageFinalPointWindow = -1f;
        public float percentageEndExplore = -1f;

        // Public Methods  #####################################################

        public void FinishSession() {
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
            intervalStartExplore = CalcInterval(
                timingLevelStart,
                timingMusicStart
            );
            intervalMusicPlay = CalcInterval(timingMusicStart, timingMusicEnd);
            intervalFinalPointWindow = CalcInterval(
                timingMusicEnd,
                timingWindowClose
            );
            intervalEndExplore = CalcInterval(
                timingWindowClose,
                timingLevelEnd
            );
            intervalTotal = CalcInterval(timingLevelStart, timingLevelEnd);

            // percentage
            percentageStartExplore = CalcPercentage(intervalStartExplore);
            percentageMusicPlay = CalcPercentage(intervalMusicPlay);
            percentageFinalPointWindow = CalcPercentage(intervalFinalPointWindow);
            percentageEndExplore = CalcPercentage(intervalEndExplore);
        }

        // Constructor  ########################################################
        public LevelMetric() {
            fps = new List<int>();
        }

        // private methods  ####################################################

        private static float CalcInterval(float from, float to) {
            if (from == -1f || to == -1f) {
                return -1f;
            }

            float value = to - from;
            return value < 0 ? -1f : value;
        }

        private float CalcPercentage(float interval) {
            if (intervalTotal == -1f || interval == -1f) {
                return -1f;
            }

            return interval / intervalTotal;
        }
    }
}

#endif
