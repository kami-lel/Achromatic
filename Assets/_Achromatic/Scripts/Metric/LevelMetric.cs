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

        // Public Methods  #####################################################

        public void FinishSession() {
            if (fps.Count <= 0) {
                return;
            }

            fpsMin = fps.Min();
            fpsMax = fps.Max();

            float total = 0f;
            for (int i = 0; i < fps.Count; i++) {
                total += fps[i];
            }
            fpsMean = (float)total / fps.Count;
        }

        // Constructor  ########################################################
        public LevelMetric() {
            fps = new List<int>();
        }
    }
}

#endif