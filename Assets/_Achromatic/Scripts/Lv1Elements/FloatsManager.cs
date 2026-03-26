using UnityEngine;

public class FloatingObject: MonoBehaviour {

    // Inspector Fields  #######################################################

    [SerializeField] private Sprite[] sprites;

    [Header("Vertical Motion")]
    [SerializeField] private float verticalAmplitude = 1f;
    [SerializeField] private float verticalFrequency = 1f;

    [Header("Horizontal Motion")]
    [SerializeField] private float horizontalAmplitude = 0.3f;
    [SerializeField] private float horizontalFrequency = 0.5f;

    [Header("Randomness")]
    [SerializeField] private float noiseStrength = 0.1f;
    [SerializeField] private float noiseSpeed = 0.5f;

    [Header("Rotation")]
    [SerializeField] private float rotationAmount = 5f;
    [SerializeField] private float rotationSpeed = 0.5f;

    private Vector3 startPosition;
    private float noiseOffsetX;
    private float noiseOffsetY;

    // MonoBehavior Lifecycle  #################################################

    void Awake() {
        startPosition = transform.position;

        // Random offsets so multiple objects don't move in sync
        noiseOffsetX = Random.Range(0f, 100f);
        noiseOffsetY = Random.Range(0f, 100f);
    }

    void Update() {
        float time = Time.time;

        // Primary motion (sine waves)
        float vertical = Mathf.Sin(time * verticalFrequency) * verticalAmplitude;
        float horizontal = Mathf.Sin(time * horizontalFrequency) * horizontalAmplitude;

        // Perlin noise for subtle randomness
        float noiseX = (Mathf.PerlinNoise(time * noiseSpeed, noiseOffsetX) - 0.5f) * 2f * noiseStrength;
        float noiseY = (Mathf.PerlinNoise(noiseOffsetY, time * noiseSpeed) - 0.5f) * 2f * noiseStrength;

        // Apply position
        transform.position = startPosition + new Vector3(
            horizontal + noiseX,
            vertical + noiseY,
            0f
        );

        // Subtle rotation to sell the float
        float angle = Mathf.Sin(time * rotationSpeed) * rotationAmount;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}