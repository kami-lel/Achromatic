#if UNITY_EDITOR || DEVELOPMENT_BUILD

using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Achromatic.Scripts.Scores;

namespace Assets._Achromatic.Scripts.Metric {
    [Serializable]
    public class LevelMetric {
        [Serializable]
        public class Timing {
            public float levelStart = -1f;
            public float musicStart = -1f;
            public float musicEnd = -1f;
            public float windowClose = -1f;
            public float levelEnd = -1f;
        }

        [Serializable]
        public class Interval {
            public float startExplore = -1f;
            public float musicPlay = -1f;
            public float finalPointWindow = -1f;
            public float endExplore = -1f;
            public float total = -1f;
        }

        [Serializable]
        public class HitsCount {
            public int hitPerfect = 0;
            public int hitGreat = 0;
            public int hitGood = 0;
            public int hitMiss = 0;
            public int hitIncorrect = 0;

            public int timeEarly = 0;
            public int timeLate = 0;

            public int totalHits = 0;

            public HitsCount(Score score) {
                foreach (var entry in score.hitCnt) {
                    Hit hit = entry.Key;
                    int v = entry.Value;

                    totalHits += v;

                    // hit type
                    if ((hit & Hit.PERFECT) != 0) {
                        hitPerfect += v;
                    } else if ((hit & Hit.GREAT) != 0) {
                        hitGreat += v;
                    } else if ((hit & Hit.GOOD) != 0) {
                        hitGood += v;
                    } else if ((hit & Hit.MISS) != 0) {
                        hitMiss += v;
                    } else if ((hit & Hit.INCORRECT) != 0) {
                        hitIncorrect += v;
                    }

                    // hit timing
                    if ((hit & Hit.EARLY) != 0) {
                        timeEarly += v;
                    } else if ((hit & Hit.LATE) != 0) {
                        timeLate += v;
                    }
                }
            }
        }

        public HitsCount hits;


        // Public Members  #####################################################

        public List<int> fps;

        public int fpsMax = -1;
        public int fpsMin = -1;
        public float fpsMean = -1f;

        public float totalScore = -1f;
        public int maxCombo = -1;

        public Timing timings;
        public Interval intervals;
        public HitsCount hitsCount;

        // Public Methods  #####################################################

        public void LogScore(Score score) {
            totalScore = score.runningScore;
            maxCombo = score.maxCombo;
            hits = new HitsCount(score);
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
        }

        // Constructor  ########################################################
        public LevelMetric() {
            fps = new List<int>();

            timings = new Timing();
            intervals = new Interval();
        }

        // private methods  ####################################################

        private static float CalcInterval(float from, float to) {
            if (from == -1f || to == -1f) {
                return -1f;
            }

            float value = to - from;
            return value < 0 ? -1f : value;
        }
    }
}

#endif
