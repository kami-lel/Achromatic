
using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {
    public class MusicManager: MonoBehaviour {

        // Public API  #########################################################

        public float Time {
            get {
                return pseudoAudioPlugin.preludeAndMain.time;
            }
        }

        public void Start() {
            pseudoAudioPlugin.bgm.loop = true;
            pseudoAudioPlugin.bgm.Play();
        }

        public void StartVamp() {
            pseudoAudioPlugin.bgm.Stop();
            pseudoAudioPlugin.vamp.loop = true;
            pseudoAudioPlugin.vamp.Play();
        }

        public void StopVamp() {
            pseudoAudioPlugin.vamp.Stop();
            pseudoAudioPlugin.bgm.Play();
        }

        public void UpdateVampVolume(float vol) {
            pseudoAudioPlugin.vamp.volume = vol;
        }

        public void StartPreludeThenMainPiece() {
            pseudoAudioPlugin.bgm.Stop();
            pseudoAudioPlugin.vamp.Stop();
            pseudoAudioPlugin.preludeAndMain.Play();
        }

        public void DebugStartMusic(int debugMusicStaringBar) {
            /* HACK
            float startTime = (debugMusicStaringBar - 1.0f)
                                * p.beatmap.meta.beatPerBar
                                * (60.0f / p.beatmap.meta.tempo)
                                + p.beatmap.meta.preludeSeconds;

            pseudoAudioPlugin.preludeAndMain.time = startTime;

            // start the music
            pseudoAudioPlugin.preludeAndMain.Play();
            */
        }

        // Inspector Fields  #######################################################

        [SerializeField]
        private PseudoAudioPlugin pseudoAudioPlugin;

    }
}