
using Assets._Achromatic.Scripts.Beatmaps;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {
    [RequireComponent(typeof(Beatmap))]
    public class MusicManager: MonoBehaviour {

        // Public API  #########################################################

        public float Time {
            get {
                return preludeAndMain.time;
            }
        }

        public void Start() {
            bgm.loop = true;
            bgm.Play();
        }

        public void StartVamp() {
            bgm.Stop();
            vamp.loop = true;
            vamp.Play();
        }

        public void StopVamp() {
            vamp.Stop();
            bgm.Play();
        }

        public void UpdateVampVolume(float vol) {
            vamp.volume = vol;
        }

        public void StartPreludeThenMainPiece() {
            bgm.Stop();
            vamp.Stop();
            preludeAndMain.Play();
        }

        public void DebugStartMusic(int debugMusicStaringBar) {
            float startTime = (debugMusicStaringBar - 1.0f) *
                    beatmap.meta.beatPerBar *
                    (60.0f / beatmap.meta.tempo) +
                    beatmap.meta.preludeSeconds;

            preludeAndMain.time = startTime;

            // start the music
            preludeAndMain.Play();
        }

        // Inspector Fields  ###################################################

        [SerializeField]
        public AudioSource bgm;

        [SerializeField]
        public AudioSource vamp;

        [SerializeField]
        public AudioSource preludeAndMain;

        // Monobehavior Lifecycle  #############################################
        private void Awake() {
            // test inspector fields  ------------------------------------------
            if (bgm == null) {
                Debug.LogError("must assign: BGM");
                return;
            }
            if (vamp == null) {
                Debug.LogError("must assign: Vamp");
                return;
            }
            if (preludeAndMain == null) {
                Debug.LogError("must assign: Prelude And Main");
                return;
            }

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