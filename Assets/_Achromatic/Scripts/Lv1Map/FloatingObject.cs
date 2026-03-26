using UnityEngine;

namespace Assets._Achromatic.Scripts.Lv1Map {

    [RequireComponent(typeof(SpriteRenderer))]
    public class FloatingObject: MonoBehaviour {
        // Inspector Fields  ###################################################
        [SerializeField] private Sprite[] sprites;

        [Header("Float Radius")]
        [SerializeField] private Vector2 verticalRadiusRange = new Vector2(0.4f, 1.0f);
        [SerializeField] private Vector2 horizontalRadiusRange = new Vector2(0.1f, 0.4f);

        [Header("Drift Speed")]
        [SerializeField] private Vector2 driftSpeedRange = new Vector2(0.1f, 0.4f);

        [Header("Smoothing")]
        [SerializeField] private Vector2 smoothTimeRange = new Vector2(0.8f, 2.0f);

        [Header("Rotation")]
        [SerializeField] private Vector2 rotationAngleRange = new Vector2(2f, 8f);
        [SerializeField] private Vector2 rotationSpeedRange = new Vector2(0.1f, 0.4f);

        // Private Runtime Values  #############################################
        private float verticalRadius;
        private float horizontalRadius;
        private float driftSpeed;
        private float smoothTime;
        private float rotationAngle;
        private float rotationSpeed;

        private Vector3 startPosition;
        private Vector3 currentVelocity;
        private float currentAngle;
        private float angleVelocity;

        private float noiseOffsetX;
        private float noiseOffsetY;
        private float noiseOffsetAngle;

        // MonoBehavior Lifecycle  #############################################
        void Awake() {
            startPosition = transform.position;

            noiseOffsetX = Random.Range(0f, 100f);
            noiseOffsetY = Random.Range(0f, 100f);
            noiseOffsetAngle = Random.Range(0f, 100f);

            verticalRadius = Random.Range(verticalRadiusRange.x, verticalRadiusRange.y);
            horizontalRadius = Random.Range(horizontalRadiusRange.x, horizontalRadiusRange.y);
            driftSpeed = Random.Range(driftSpeedRange.x, driftSpeedRange.y);
            smoothTime = Random.Range(smoothTimeRange.x, smoothTimeRange.y);
            rotationAngle = Random.Range(rotationAngleRange.x, rotationAngleRange.y);
            rotationSpeed = Random.Range(rotationSpeedRange.x, rotationSpeedRange.y);

            if (sprites != null && sprites.Length > 0)
                GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
            else
                Debug.LogWarning("No sprites assigned!", this);
        }

        void Update() {
            float time = Time.time;

            float noiseX = Mathf.PerlinNoise(time * driftSpeed, noiseOffsetX) * 2f - 1f;
            float noiseY = Mathf.PerlinNoise(noiseOffsetY, time * driftSpeed) * 2f - 1f;
            float noiseA = Mathf.PerlinNoise(time * rotationSpeed, noiseOffsetAngle) * 2f - 1f;

            Vector3 targetOffset = new Vector3(
                noiseX * horizontalRadius,
                noiseY * verticalRadius,
                0f
            );

            transform.position = Vector3.SmoothDamp(
                transform.position,
                startPosition + targetOffset,
                ref currentVelocity,
                smoothTime
            );

            float targetAngle = noiseA * rotationAngle;
            currentAngle = Mathf.SmoothDampAngle(
                currentAngle,
                targetAngle,
                ref angleVelocity,
                smoothTime * 1.5f
            );

            transform.rotation = Quaternion.Euler(0f, 0f, currentAngle);
        }
    }
}