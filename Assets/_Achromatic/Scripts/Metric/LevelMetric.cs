#if UNITY_EDITOR || DEVELOPMENT_BUILD


using System;
using System.Collections.Generic;

namespace Assets._Achromatic.Scripts.Metric {

    [Serializable]
    public class LevelMetric {

        // Public Members  #####################################################
        public List<int> fps;

        // TODO

        // Constructor  ########################################################
        public LevelMetric() {
            fps = new List<int>();
        }
    }
}

#endif