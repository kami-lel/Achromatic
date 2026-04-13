
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
            if (fps < 10) {
                return;
            }

            currentLevelMetric.fps.Add(fps);
        }

        public void LogScore(Score score) {
            currentLevelMetric.totalScore = score.runningScore;
            currentLevelMetric.maxCombo = score.maxCombo;

            // TODO save all hits
            // TODO metrics: deltas
            // TODO metrics: hit / miss ratio per part
        }

        // log sequence key points  ============================================

        public void LogLevelStart() {
            level += 1;
            currentLevelMetric = session.levels[level];

            currentLevelMetric.timingLevelStart = Time.time;
        }

        public void LogMusicStart() {
            currentLevelMetric.timingMusicStart = Time.time;
        }

        public void LogMusicEnd() {
            currentLevelMetric.timingMusicEnd = Time.time;
        }

        public void LogWindowClose() {
            currentLevelMetric.timingWindowClose = Time.time;
        }

        public void LogLevelEnd() {
            currentLevelMetric.timingLevelEnd = Time.time;
        }


        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // singleton logic  ------------------------------------------------
            I = this;

            session = new MetricSession(Application.version);
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

        // private methods  ####################################################
        private void FinishSession() {
            foreach (LevelMetric level in session.levels) {
                level.FinishSession();
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
