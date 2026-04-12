
using System;
using System.Collections.Generic;
using System.IO;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Metric {
    [DefaultExecutionOrder(-99)]
    public class Metrics: MonoBehaviour {
#if UNITY_EDITOR || DEVELOPMENT_BUILD

        // Public Members  #####################################################

        // singleton
        public static Metrics I {
            get; private set;
        }

        // Public Members  #####################################################

        public void LogFPS(int fps) {
            if (fps == 1) {
                return;
            }

            currentLevelMetric.fps.Add(fps);
        }

        public void LogSequenceKeyPoint(SequenceKeyPoint keyPoint) {
            string timingKey = "";
            string intervalKey = "";

            // FIXME correct per step

            switch (keyPoint) {
            case SequenceKeyPoint.LEVEL_START:
                level += 1;
                currentLevelMetric = session.levels[level];
                timingKey = "LevelStart";
                break;

            case SequenceKeyPoint.MAIN_START:
                timingKey = "MusicPlayStart";
                intervalKey = "BeginningExplorePlay";
                break;

            case SequenceKeyPoint.MAIN_FINISHED:
                timingKey = "MusicPlayFinished";
                intervalKey = "MusicPlay";
                break;

            case SequenceKeyPoint.CLOSE_WINDOW:
                timingKey = "CloseFinalScoreWindow";
                intervalKey = "FinalScoreWindow";
                break;

            case SequenceKeyPoint.LEVEL_END:
                timingKey = "FinishLevel";
                intervalKey = "EndingExplorePlay";
                break;

            default:
                break;
            }

            float timing = Time.time;
            float interval = timing - lastTiming;

            if (timingKey != "") {
                timings[timingKey] = timing;
            }
            if (intervalKey != "") {
                intervals[intervalKey] = interval;
            }

        }

        public void LogMusicPlay(Score score) {
            currentLevelMetric.totalScore = score.runningScore;
            currentLevelMetric.maxCombo = score.maxCombo;

            // TODO save all hits
            // TODO metrics: deltas
            // TODO metrics: hit / miss ratio per part
        }

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // singleton logic  ------------------------------------------------
            I = this;

            session = new MetricSession(Application.version);
            timings = new Dictionary<string, float>();
            intervals = new Dictionary<string, float>();
        }

        private void OnDisable() {
            FinishSession();
        }

        private void OnApplicationQuit() {
            FinishSession();
        }

        // private members  ####################################################
        private int level = -1;
        private float lastTiming = 0f;

        private MetricSession session;
        private LevelMetric currentLevelMetric;
        private Dictionary<string, float> timings;
        private Dictionary<string, float> intervals;

        // private methods  ####################################################
        private void FinishSession() {
            foreach (LevelMetric level in session.levels) {
                level.FinishSession(timings);
            }

            // save metric  ****************************************************
            string json = JsonUtility.ToJson(session, prettyPrint: true);
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
            string filename = $"Achromatic.Metric.{timestamp}.json";
            string path = Path.Combine(
                Application.persistentDataPath,
                filename
            );

            File.WriteAllText(path, json);
        }

#endif
    }
}
