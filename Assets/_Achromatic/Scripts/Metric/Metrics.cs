// TODO metrics save to file

namespace Metric
{
    public class Metrics : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD

        // Public Members  #####################################################

        // singleton
        public static Metrics I { get; private set; }

        // Public Members  #####################################################

        public void LogFPS(int fps) {
            // TODO
        }

        public void LogSequenceKeyPoint(SequenceKeyPoint keyPoint) {
            float timing = Time.time;
            float interval = timing - lastTiming;

            // TODO metrics: total time
            // TODO portion of game play

            switch (keyPoint)
            {
                case SequenceKeyPoint.LEVEL_START:
                    level += 1;
                    break;

                default:
                    break;
            }
        }

        public void LogMusicPlay(Score score)
        {
            // TODO metrics: deltas
            // TODO metrics: hit / miss ratio per part
        }

        // MonoBehavior Lifecycle  #############################################

        private void Awake()
        {
            // singleton logic  ------------------------------------------------
            I = this;
        }

        // private members  ####################################################
        private int level = 0;
        private float lastTiming;

#endif
    }
}
