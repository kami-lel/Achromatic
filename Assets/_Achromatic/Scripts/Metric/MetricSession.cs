#if UNITY_EDITOR || DEVELOPMENT_BUILD

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Metric {
    [Serializable]
    public class MetricSession {
        public Dictionary<int, LevelMetric> levels;

        public string gameVersion;

        public MetricSession(string version) {
            gameVersion = version;

            levels = new Dictionary<int, LevelMetric>
            {
                { 1, new LevelMetric() },
                { 2, new LevelMetric() },
                { 3, new LevelMetric() },
            };
        }
    }
}

#endif
