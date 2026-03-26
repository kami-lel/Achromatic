using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class GameTitleScript: MonoBehaviour {
    // inspector fields ########################################################
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float playerDeltaX = 15f;   // how far player must move in x
    [SerializeField] private float titleDeltaY = 20f;    // how far the title moves up in y
    [SerializeField] private float smoothTime = 0.3f;   // lower = snappier, higher = floatier

    // MonoBehaviour Lifecycle #################################################
    void Start() {
        rectTransform = GetComponent<TextMeshProUGUI>().rectTransform;
        initialY = rectTransform.anchoredPosition.y;

        if (playerTransform == null) {
            Debug.LogError("GameTitleScript:\tmust set playerTransform");
            enabled = false;
            return;
        }

        startPlayerX = playerTransform.position.x;
        maxPlayerX = startPlayerX;
    }

    void Update() {
        // Track furthest-right player X so progress never decreases.
        maxPlayerX = Mathf.Max(maxPlayerX, playerTransform.position.x);

        // Compute progress based on how far the player has moved.
        float finishPlayerX = startPlayerX + playerDeltaX;
        float desiredEndY = initialY + titleDeltaY;

        float progress = Mathf.InverseLerp(startPlayerX, finishPlayerX, maxPlayerX);
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

    // constants ##############################################################
    private const float FINISH_EPSILON = 0.5f;  // small tolerance in UI units

    // private members ########################################################
    private RectTransform rectTransform;
    private float initialY;
    private float startPlayerX;
    private float maxPlayerX;
    private float yVelocity;
}