using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("浮动设置")]
    [Tooltip("浮动的速度")]
    public float floatSpeed = 2f;

    [Tooltip("浮动的幅度（上下移动的距离）")]
    public float floatStrength = 0.5f;

    private Vector3 startPos;
    private float randomOffset;

    void Start()
    {
        startPos = transform.position;
        randomOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed + randomOffset) * floatStrength;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}