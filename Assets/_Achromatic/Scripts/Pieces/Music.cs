using Assets._Achromatic.Scripts.Beatmap;
using UnityEngine;

namespace Assets._Achromatic.Scripts.Pieces {

    public class MusicManager {

        // public API  #########################################################

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

            // FIXME debug start music
            // private void AudioStart() {
            //     // start music midpoint, for debug purpose
            //     if (debugMusicStaringBar != 0.0f) {
            //         audioSource.time = (debugMusicStaringBar - 1.0f)
            //                 * beatmap.beatPerBar
            //                 * (60.0f / beatmap.beatmapData.Tempo)
            //                 + beatmap.beatmapData.PreludeLength;
            //     }
            //     // start the music
            //     audioSource.Play();
            //     audioSource.SetScheduledEndTime(AudioSettings.dspTime + 140f);
            // }
            //
        }


        // Constructor  ########################################################
        public MusicManager(PseudoAudioPlugin pseudoAudioPlugin) {
            this.pseudoAudioPlugin = pseudoAudioPlugin;
        }

        // private members  ####################################################
        // cached references
        private readonly PseudoAudioPlugin pseudoAudioPlugin;


    }
}