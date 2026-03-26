using UnityEngine;

public class LoopingFloatingObject : MonoBehaviour
{
    [Header("浮动设置")]
    public float floatSpeed = 2f;
    public float floatStrength = 0.5f;

    [Header("循环设置")]
    [Tooltip("触发循环的距离（一般设置为摄像机宽度的一半稍微多一点）")]
    public float wrapDistanceX = 15f;

    [Tooltip("每次循环回到屏幕前方时，是否随机改变一下高度？")]
    public bool randomizeHeightOnLoop = true;

    private float startY;
    private float randomOffset;
    private Transform camTransform;

    void Start()
    {
        // 自动获取场景中的主摄像机
        camTransform = Camera.main.transform;

        // 记录初始高度和随机浮动相位
        startY = transform.position.y;
        randomOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        // 1. 计算上下浮动的 Y 轴位置
        float newY = startY + Mathf.Sin(Time.time * floatSpeed + randomOffset) * floatStrength;

        // 2. 处理左右循环
        float currentX = transform.position.x;
        float camX = camTransform.position.x;

        // 如果摄像机往右走，漂浮物落在了左边很远的地方
        if (currentX < camX - wrapDistanceX)
        {
            // 把漂浮物搬运到右边去（原距离的基础上加上两倍的跨度）
            currentX += wrapDistanceX * 2f;

            // 每次重新出现时，稍微上下偏移一下基础高度，打破规律感
            if (randomizeHeightOnLoop) startY += Random.Range(-1.5f, 1.5f);
        }
        // 如果摄像机往左走，漂浮物落在了右边很远的地方
        else if (currentX > camX + wrapDistanceX)
        {
            // 把漂浮物搬运到左边去
            currentX -= wrapDistanceX * 2f;

            if (randomizeHeightOnLoop) startY += Random.Range(-1.5f, 1.5f);
        }

        // 3. 更新物体的最终位置
        transform.position = new Vector3(currentX, newY, transform.position.z);
    }
}