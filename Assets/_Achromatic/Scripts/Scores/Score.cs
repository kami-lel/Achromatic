using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using Assets._Achromatic.Scripts.Beatmaps;

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
                combo = 0;  // miss, reset combo
            } else {
                combo += 1;
                if (combo > maxCombo) {
                    maxCombo = combo;
                }
            }

            Debug.Log("Score.Record:"
                    + $"\tjudge: {hit}"
                    + $"\tscore: {runningScore}"
                    + $"\tcombo: {combo}"
                    );

            // update indicators  ----------------------------------------------
            // combo indicator
            if (comboIndicator != null) {
                comboIndicator.text = $"{combo}";
            }
            // running score
            if (runningScoreIndicator != null) {
                runningScoreIndicator.text = $"{(int)runningScore}";
            }
            // hit type indicator
            if (hitTypeIndicator != null) {
                hitTypeIndicator.Show(hit);
            }
            // score addition indicator
            if (scoreAdditionIndicator != null) {
                scoreAdditionIndicator.Show((int)scoreAddition);
            }
        }

        // MonoBehavior Lifecycle  #############################################

        private void Awake() {
            // caching reference of piece  -------------------------------------
            beatmap = GetComponent<Beatmap>();
            if (beatmap == null) {
                Debug.LogError("fail to get: Beatmap");
            }

            // init resultCnt  -------------------------------------------------
            hitCnt = new Dictionary<Hit, int>();
            foreach (Hit result
                    in Enum.GetValues(typeof(Hit))) {
                hitCnt[result] = 0;  // filled w/ 0
            }

            // init combo  -----------------------------------------------------
            combo = 0;
            maxCombo = 0;

            // init indicators
            SceneManager.sceneLoaded += HandleInitIndicators;
        }

        private void Start() {
            // init perResultScores  -------------------------------------------
            perfectScore = TOTAL_SCORES / beatmap.NotesCount;
            greatScore = perfectScore * 0.7f;
            goodScore = perfectScore * 0.3f;
        }

        // event handlers  #####################################################

        private void HandleInitIndicators(Scene scene, LoadSceneMode mode) {
            // combo indicator
            GameObject comboGO = GameObject.FindWithTag("ComboIndicator");
            if (comboGO != null) {
                comboIndicator = comboGO.GetComponent<TextMeshProUGUI>();
            }

            // running score indicator
            GameObject runningGO = GameObject.FindWithTag("RunningScoreIndicator");
            if (comboGO != null) {
                runningScoreIndicator = runningGO.GetComponent<TextMeshProUGUI>();
            }

            // hit type indicator
            GameObject hitTypeGO = GameObject.FindWithTag("HitIndicator");
            if (hitTypeGO != null) {
                hitTypeIndicator = hitTypeGO.GetComponent<HitTypeIndicatorScript>();
            }
            // score addition indicator
            GameObject scoreAddGO = GameObject.FindWithTag("ScoreAdditionIndicator");
            if (scoreAddGO != null) {
                scoreAdditionIndicator = scoreAddGO.GetComponent<ScoreAdditionIndicatorScript>();
            }

            // print error if fail to find by tags
            if (comboIndicator == null) {
                Debug.LogError("Score: fail to find Combo Indicator");
            }
            if (runningScoreIndicator == null) {
                Debug.LogError("Score: fail to find Running Score Indicator");
            }
            if (hitTypeIndicator == null) {
                Debug.LogError("Score: fail to find Hit Type Indicator");
            }
            if (scoreAdditionIndicator == null) {
                Debug.LogError("Score: fail to find Score Addition Indicator");
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
        private TextMeshProUGUI comboIndicator;
        private TextMeshProUGUI runningScoreIndicator;
        private HitTypeIndicatorScript hitTypeIndicator;
        private ScoreAdditionIndicatorScript scoreAdditionIndicator;
        private Beatmap beatmap;
    }

}