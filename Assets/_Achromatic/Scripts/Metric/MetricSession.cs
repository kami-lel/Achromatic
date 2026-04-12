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
        }
    }
}

#endif
