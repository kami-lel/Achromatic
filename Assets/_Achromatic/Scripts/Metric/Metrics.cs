// TODO metrics save to file

using UnityEngine;
using Assets._Achromatic.Scripts.Scores;
using System.IO;

namespace Assets._Achromatic.Scripts.Metric {
    public class Metrics: MonoBehaviour {
#if UNITY_EDITOR || DEVELOPMENT_BUILD

        // Public Members  #####################################################

        // singleton
        public static Metrics I {
            get; private set;
        }

        // Public Members  #####################################################

        public void LogFPS(int fps) {
            session.fps.Add(fps);
        }

        public void LogSequenceKeyPoint(SequenceKeyPoint keyPoint) {
            float timing = Time.time;
            float interval = timing - lastTiming;

            // TODO metrics: total time
            // TODO portion of game play

            switch (keyPoint) {
            case SequenceKeyPoint.LEVEL_START:
                level += 1;
                break;

            default:
                break;
            }
        }

        public void LogMusicPlay(Score score) {
            // TODO metrics: deltas
            // TODO metrics: hit / miss ratio per part
        }

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // singleton logic  ------------------------------------------------
            I = this;

            session = new MetricSession();
        }

        private void OnDisable() {
            SaveSession();
        }

        private void OnApplicationQuit() {
            SaveSession();
        }

        // private members  ####################################################
        private int level = 0;
        private float lastTiming;
        private MetricSession session;

        // private methods  ####################################################
        private void SaveSession() {
            string json = JsonUtility.ToJson(session, prettyPrint: true);
            string path = Path.Combine(
                Application.persistentDataPath,
                "metric_session.json"
            );
            // TODO add name

            File.WriteAllText(path, json);
        }

#endif
    }
}
