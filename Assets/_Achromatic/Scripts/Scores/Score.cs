using System;
using System.Collections.Generic;
using System.Linq;
using Assets._Achromatic.Scripts.Beatmaps;
using Assets._Achromatic.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// todo improve looking of indicators

namespace Assets._Achromatic.Scripts.Scores {
    [RequireComponent(typeof(Beatmap))]
    public class Score: MonoBehaviour {
        // Public Members  #####################################################

        /// <summary>
        /// running score, maybe lower than actual points
        /// </summary>
        [NonSerialized]
        public float runningScore;

        /// <summary>
        /// number of current combos
        /// </summary>
        [NonSerialized]
        public int combo;

        /// <summary>
        /// number of max combos
        /// </summary>
        [NonSerialized]
        public int maxCombo;

        /// <summary>
        /// count each type of result
        /// </summary>
        public Dictionary<Hit, int> hitCnt;

        public event Action<int, int, int> OnScoreChange;

        // Public Methods  #####################################################

        public void Record(Hit hit) {
            // save results
            hitCnt[hit] += 1;

            float scoreAddition = 0;
            // update running score
            if ((hit & Hit.PERFECT) != 0) {
                scoreAddition = perfectScore;
            } else if ((hit & Hit.GREAT) != 0) {
                scoreAddition = greatScore;
            } else if ((hit & Hit.GOOD) != 0) {
                scoreAddition = goodScore;
            }
            runningScore += scoreAddition;

            // record combo
            if ((hit & Hit.NO_SCORE) != 0) {
                combo = 0; // miss, reset combo
            } else {
                combo += 1;
                if (combo > maxCombo) {
                    maxCombo = combo;
                }
            }

            Debug.Log(
                "Score.Record:"
                    + $"\tjudge: {hit}"
                    + $"\tscore: {runningScore}"
                    + $"\tcombo: {combo}"
            );

            // update indicators  ----------------------------------------------
            // hit type indicator
            if (hitTypeIndicator != null) {
                hitTypeIndicator.Show(hit);
            }

            OnScoreChange?.Invoke(combo, (int)runningScore, (int)scoreAddition);
        }

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // caching reference of piece  -------------------------------------
            beatmap = GetComponent<Beatmap>();
            if (beatmap == null) {
                Debug.LogError("fail to get: Beatmap", this);
            }

            // init resultCnt  -------------------------------------------------
            hitCnt = new Dictionary<Hit, int>();
            foreach (Hit result in Enum.GetValues(typeof(Hit))) {
                hitCnt[result] = 0; // filled w/ 0
            }

            // init combo  -----------------------------------------------------
            combo = 0;
            maxCombo = 0;

            // init indicators
            SceneManager.sceneLoaded += HandleInitIndicators;
        }

        private void Start() {
            // init perResultScores  -------------------------------------------
            perfectScore = TOTAL_SCORES / beatmap.notesQ.Count();
            greatScore = perfectScore * 0.7f;
            goodScore = perfectScore * 0.3f;
        }

        // event handlers  #####################################################

        private void HandleInitIndicators(Scene scene, LoadSceneMode mode) {
            // todo using OnScoreChange instead
            // hit type indicator
            GameObject hitTypeGO = GameObject.FindWithTag("HitIndicator");
            if (hitTypeGO != null) {
                hitTypeIndicator =
                    hitTypeGO.GetComponent<HitTypeIndicatorScript>();
            }
            if (hitTypeIndicator == null) {
                Debug.LogError("Score: fail to find Hit Type Indicator", this);
            }
        }

        // constants  ##########################################################

        /// <summary>
        /// total score possible for a piece
        /// </summary>
        private const int TOTAL_SCORES = 10000;

        // private members  ####################################################
        private float perfectScore;
        private float greatScore;
        private float goodScore;

        // cached references
        private HitTypeIndicatorScript hitTypeIndicator;
        private Beatmap beatmap;
    }
}
