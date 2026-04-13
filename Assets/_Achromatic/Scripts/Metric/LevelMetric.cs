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
        public class HitsByType {
            public int perfect = 0;
            public int great = 0;
            public int good = 0;
            public int miss = 0;
            public int incorrect = 0;

            public HitsByType(Score score) {
                foreach (var entry in score.hitCnt) {
                    Hit hit = entry.Key;
                    int v = entry.Value;

                    // hit type
                    if ((hit & Hit.PERFECT) != 0) {
                        perfect += v;
                    } else if ((hit & Hit.GREAT) != 0) {
                        great += v;
                    } else if ((hit & Hit.GOOD) != 0) {
                        good += v;
                    } else if ((hit & Hit.MISS) != 0) {
                        miss += v;
                    } else if ((hit & Hit.INCORRECT) != 0) {
                        incorrect += v;
                    }
                }

            }
        }

        [Serializable]
        public class HitsByTiming {

            public int timeEarly = 0;
            public int timeLate = 0;

            public HitsByTiming(Score score) {

                foreach (var entry in score.hitCnt) {
                    Hit hit = entry.Key;
                    int v = entry.Value;

                    // hit timing
                    if ((hit & Hit.EARLY) != 0) {
                        timeEarly += v;
                    } else if ((hit & Hit.LATE) != 0) {
                        timeLate += v;
                    }
                }
            }
        }

        [Serializable]
        public class HitsCount {

            public int noHit = 0;
            public int earlyMiss = 0;
            public int lateMiss = 0;
            public int incorrect = 0;
            public int earlyGreat = 0;
            public int lateGreat = 0;
            public int earlyGood = 0;
            public int lateGood = 0;
            public int earlyPerfect = 0;
            public int latePerfect = 0;

            public HitsCount(Score score) {
                foreach (var entry in score.hitCnt) {
                    Hit hit = entry.Key;
                    int v = entry.Value;

                    switch (hit) {
                    case Hit.NO_HIT:
                        noHit += v;
                        break;

                    case Hit.EARLY_MISS:
                        earlyMiss += v;
                        break;

                    case Hit.LATE_MISS:
                        lateMiss += v;
                        break;

                    case Hit.INCORRECT:
                        incorrect += v;
                        break;

                    case Hit.EARLY_GOOD:
                        earlyGood += v;
                        break;

                    case Hit.LATE_GOOD:
                        lateGood += v;
                        break;

                    case Hit.EARLY_GREAT:
                        earlyGreat += v;
                        break;

                    case Hit.LATE_GREAT:
                        lateGreat += v;
                        break;

                    case Hit.EARLY_PERFECT:
                        earlyPerfect += v;
                        break;

                    case Hit.LATE_PERFECT:
                        latePerfect += v;
                        break;

                    default:
                        break;
                    }
                }
            }
        }

        // Public Members  #####################################################

        public List<int> fps;

        public int fpsMax = -1;
        public int fpsMin = -1;
        public float fpsMean = -1f;

        public float totalScore = -1f;
        public int maxCombo = -1;

        public Timing timings;
        public Interval intervals;
        public HitsByType hitsByType;
        public HitsByTiming hitsByTiming;
        public HitsCount hitsCount;

        // Public Methods  #####################################################

        public void LogScore(Score score) {
            totalScore = score.runningScore;
            maxCombo = score.maxCombo;

            hitsByType = new HitsByType(score);
            hitsByTiming = new HitsByTiming(score);
            hitsCount = new HitsCount(score);
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
