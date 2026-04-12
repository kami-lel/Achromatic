using System;
using System.Collections;
using Assets._Achromatic.Scripts.Beatmaps;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {
    [RequireComponent(typeof(Beatmap))]
    [RequireComponent(typeof(Ender))]
    public class MusicManager: MonoBehaviour {
        // Public API  #########################################################

        public float Time {
            get {
                return preludeAndMain.time;
            }
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
            StartCoroutine(MusicFinishingCoroutine());
        }

        public void DebugStartMusic(int debugMusicStaringBar) {
            bgm.Stop();
            vamp.Stop();
            float startTime =
                (debugMusicStaringBar - 1.0f)
                    * beatmap.meta.beatPerBar
                    * (60.0f / beatmap.meta.tempo)
                + beatmap.meta.preludeSeconds;

            preludeAndMain.time = startTime;

            // start the music
            preludeAndMain.Play();
        }

        // Inspector Fields  ###################################################

        [SerializeField]
        private AudioSource bgm;

        [SerializeField]
        private AudioSource vamp;

        [SerializeField]
        private AudioSource preludeAndMain;

        // Monobehavior Lifecycle  #############################################
        private void Awake() {
            // test inspector fields  ------------------------------------------
            if (bgm == null) {
                Debug.LogError("must assign: BGM", this);
                return;
            }
            if (vamp == null) {
                Debug.LogError("must assign: Vamp", this);
                return;
            }
            if (preludeAndMain == null) {
                Debug.LogError("must assign: Prelude And Main", this);
                return;
            }

            // caching reference of piece  -------------------------------------
            beatmap = GetComponent<Beatmap>();
            if (beatmap == null) {
                Debug.LogError("fail to get: Beatmap", this);
            }

            ender = GetComponent<Ender>();
            if (ender == null) {
                Debug.LogError("fail to get: Ender", this);
            }
        }

        private void Start() {
            bgm.loop = true;
            bgm.Play();
        }

        // private members  ####################################################
        // cached references
        private Beatmap beatmap;
        private Ender ender;


        // private methods  ####################################################
        private IEnumerator MusicFinishingCoroutine() {
            yield return new WaitForSeconds(preludeAndMain.clip.length);
            ender.FinishMain();
        }
    }
}
