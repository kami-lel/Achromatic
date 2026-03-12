using UnityEngine;
using TMPro;

// Todo smooth movement w/ inertia

[RequireComponent(typeof(TextMeshProUGUI))]
public class GameTitleScript : MonoBehaviour
{
    // inspector fields #######################################################
    [SerializeField]
    private Transform playerTransform;

    [SerializeField]
    private float playerDeltaX = 15f;  // how far player must move in x

    [SerializeField]
    private float titleDeltaY = 20f;  // how far the title moves up in y

    // MonoBehaviour Lifecycle ################################################
    void Start()
    {
        rectTransform =
            GetComponent<TextMeshProUGUI>().rectTransform;  // cache rect
        initialY = rectTransform.anchoredPosition.y;  // read start y

        if (playerTransform == null)
        {
            Debug.LogError("GameTitleScript:\tmust set playerTransform");  // log
            enabled = false;  // disable this component to avoid updates
            return;
        }

        startPlayerX = playerTransform.position.x;  // record start x
        maxPlayerX = startPlayerX;  // begin tracking max player x
    }

    void Update()
    {
        // track furthest right player X so title never moves down
        maxPlayerX = Mathf.Max(maxPlayerX, playerTransform.position.x);

        // compute finish positions using deltas
        float finishPlayerX = startPlayerX + playerDeltaX;
        float desiredEndY = initialY + titleDeltaY;

        // compute progress 0..1 based on how far player has moved
        float progress = Mathf.InverseLerp(
            startPlayerX, finishPlayerX, maxPlayerX);

        // desired Y based on progress
        float desiredY = Mathf.Lerp(initialY, desiredEndY, progress);

        // never move the title down
        float currentY = rectTransform.anchoredPosition.y;
        float newY = Mathf.Max(currentY, desiredY);

        // smoothly move toward the new Y
        Vector2 currentPos = rectTransform.anchoredPosition;
        Vector2 targetPos = new(currentPos.x, newY);
        float step = MOVE_SPEED * Time.deltaTime;
        rectTransform.anchoredPosition =
            Vector2.MoveTowards(currentPos, targetPos, step);

        // detect finish and disable this component when reached
        bool reachedProgress = progress >= 1f - Mathf.Epsilon;
        bool reachedY = Mathf.Abs(
            rectTransform.anchoredPosition.y - desiredEndY)
            <= FINISH_EPSILON;
        if (reachedProgress && reachedY)
        {
            enabled = false;  // stop updating when finished
        }
    }

    // constants ##############################################################
    private const float MOVE_SPEED = 200f;  // units per second for UI smoothing
    private const float FINISH_EPSILON = 0.5f;  // small tolerance in UI units

    // private members ########################################################
    private RectTransform rectTransform;
    private float initialY;
    private float startPlayerX;
    private float maxPlayerX;
}