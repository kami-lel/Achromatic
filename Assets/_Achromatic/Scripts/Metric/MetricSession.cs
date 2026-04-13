#if UNITY_EDITOR || DEVELOPMENT_BUILD

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Metric {
    [Serializable]
    public class MetricSession {

        // Public Members  #####################################################
        public string gameVersion;
        public LevelMetric[] levels;

        // Constructor  ########################################################
        public MetricSession(string version) {
            gameVersion = version;

            levels = new LevelMetric[LEVEL_COUNT];
            for (int i = 0; i < LEVEL_COUNT; i++) {
                levels[i] = new LevelMetric();
            }
        }

        // constants  ##########################################################
        public const int LEVEL_COUNT = 3;
    }
}

#endif
