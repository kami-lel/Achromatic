using System;
using UnityEngine;


// TODO metrics: total time
// TODO metrics: deltas
// TODO metrics: hit / miss ratio per part

// TODO metrics save to file

namespace Metric
{
    public class Metrics : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD

        // Public Members  #####################################################

        // singleton
        public static Metrics I {
            get; private set;
        }

        // Public Members  #####################################################

        public void LogFPS(int fps) {
            // TODO
        }

        private void LogSequenceKeyPoint(SequenceKeyPoint keyPoint) {
            float timing = Time.time;
            float interval = timing - lastTiming;

            // TODO

            switch (keyPoint) {
            case SequenceKeyPoint.LEVEL_START:
                level += 1;
                break;

            default:
                break;
            }

        }

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // singleton logic  ------------------------------------------------
            I = this;

        }


        // private members  ####################################################
        private int level = 0;
        private float lastTiming;

#endif
    }
}



