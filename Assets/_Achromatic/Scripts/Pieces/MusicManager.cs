
using Assets._Achromatic.Scripts.Beatmaps;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {
    [RequireComponent(typeof(Beatmap))]
    public class MusicManager: MonoBehaviour {
        // Bug audio start is jarring, lose framerate

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
            float startTime = (debugMusicStaringBar - 1.0f)
                                * beatmap.meta.beatPerBar
                                * (60.0f / beatmap.meta.tempo)
                                + beatmap.meta.preludeSeconds;

            pseudoAudioPlugin.preludeAndMain.time = startTime;

            // start the music
            pseudoAudioPlugin.preludeAndMain.Play();
        }

        // Inspector Fields  ###################################################

        [SerializeField]
        private PseudoAudioPlugin pseudoAudioPlugin;

        // Inspector Fields  ###################################################

        private void Awake() {
            // caching reference of piece  -------------------------------------
            beatmap = GetComponent<Beatmap>();
            if (beatmap == null) {
                Debug.LogError("fail to get: Beatmap");
            }
        }

        // private members  ####################################################
        // cached references
        private Beatmap beatmap;
    }
}