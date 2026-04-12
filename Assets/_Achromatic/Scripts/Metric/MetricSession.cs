#if UNITY_EDITOR || DEVELOPMENT_BUILD

using System;
using System.Collections.Generic;

namespace Assets._Achromatic.Scripts.Metric {

    [Serializable]
    public class MetricSession {

        // TODO

        public Dictionary<int, LevelMetric> levels;

    }
}

#endif
