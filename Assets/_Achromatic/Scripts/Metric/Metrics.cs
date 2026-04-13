
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

        // log sequence key points  ============================================

        public void LogLevelStart() {
            level += 1;
            currentLevelMetric = session.levels[level];

            currentLevelMetric.timings.levelStart = Time.time;
        }

        public void LogMusicStart() {
            currentLevelMetric.timings.musicStart = Time.time;
        }

        public void LogMusicEnd(Score score) {
            currentLevelMetric.timings.musicEnd = Time.time;
            currentLevelMetric.LogScore(score);
        }

        public void LogWindowClose() {
            currentLevelMetric.timings.windowClose = Time.time;
        }

        public void LogLevelEnd() {
            currentLevelMetric.timings.levelEnd = Time.time;
            currentLevelMetric.FinishLevel();
        }


        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // singleton logic  ------------------------------------------------
            I = this;

            session = new MetricSession(Application.version);
        }

        private void OnDisable() {
            SaveMetrics();
        }

        private void OnApplicationQuit() {
            SaveMetrics();
        }

        // private members  ####################################################
        private int level = -1;

        private MetricSession session;
        private LevelMetric currentLevelMetric;

        // private methods  ####################################################
        private void SaveMetrics() {
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
