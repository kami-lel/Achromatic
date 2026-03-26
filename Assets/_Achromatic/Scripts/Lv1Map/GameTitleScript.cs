using UnityEngine;
using TMPro;

namespace Assets._Achromatic.Scripts.Lv1Map {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class GameTitleScript: MonoBehaviour {
        // inspector fields ####################################################
        [SerializeField] private Transform playerTransform;

        [Header("Player X Trigger Range")]

        [SerializeField] private float triggerWidth = 7f;
        [SerializeField] private float triggerOffsetX = -5f;

        [Header("Title Motion")]
        [SerializeField] private float titleDeltaY = 20f;  // how far the title moves up in y
        [SerializeField] private float smoothTime = 0.3f; // lower = snappier, higher = floatier

        // MonoBehaviour Lifecycle #############################################
        void Start() {
            rectTransform = GetComponent<TextMeshProUGUI>().rectTransform;
            initialY = rectTransform.anchoredPosition.y;

            if (playerTransform == null) {
                Debug.LogError("GameTitleScript:\tmust set playerTransform");
                enabled = false;
                return;
            }

            if (triggerWidth <= 0f) {
                Debug.LogError("GameTitleScript:\ttriggerWidth must be greater than zero");
                enabled = false;
                return;
            }

            // Bake the trigger bounds once, based on this object's world X plus offset.
            float centreX = transform.position.x + triggerOffsetX;
            triggerStartX = centreX - triggerWidth;
            triggerEndX = centreX + triggerWidth;

            maxPlayerX = playerTransform.position.x;
        }

        void Update() {
            // Track furthest-right player X so progress never decreases.
            maxPlayerX = Mathf.Max(maxPlayerX, playerTransform.position.x);

            float desiredEndY = initialY + titleDeltaY;

            // progress is 0 until player hits triggerStartX, then ramps to 1 at triggerEndX.
            float progress = Mathf.InverseLerp(triggerStartX, triggerEndX, maxPlayerX);
            float desiredY = Mathf.Lerp(initialY, desiredEndY, progress);

            // Never let the title move backward.
            float currentY = rectTransform.anchoredPosition.y;
            desiredY = Mathf.Max(currentY, desiredY);

            // Smoothly move Y toward desiredY.
            float newY = Mathf.SmoothDamp(
                currentY,
                desiredY,
                ref yVelocity,
                smoothTime,
                Mathf.Infinity,
                Time.deltaTime
            );

            Vector2 anchoredPos = rectTransform.anchoredPosition;
            anchoredPos.y = newY;
            rectTransform.anchoredPosition = anchoredPos;

            // Detect finish and snap exactly to final value.
            bool reachedProgress = progress >= 1f;
            bool reachedY = Mathf.Abs(newY - desiredEndY) <= FINISH_EPSILON;

            if (reachedProgress && reachedY) {
                anchoredPos.y = desiredEndY;
                rectTransform.anchoredPosition = anchoredPos;
                yVelocity = 0f;
                enabled = false;
            }
        }

        // constants ###########################################################
        private const float FINISH_EPSILON = 0.5f; // small tolerance in UI units

        // private members #####################################################
        private RectTransform rectTransform;
        private float initialY;
        private float triggerStartX;
        private float triggerEndX;
        private float maxPlayerX;
        private float yVelocity;
    }
}