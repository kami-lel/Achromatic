using UnityEngine;
using System.Collections;

public class StarGuideEffect : MonoBehaviour
{
    [Header("移动设置 (Movement)")]
    public float moveSpeed = 8.0f;

    [Header("浮动设置 (Floating)")]
    public float floatSpeed = 3.0f;
    public float floatHeight = 0.5f;

    [Header("星星呼吸设置 (Star Pulse)")]
    public float flashSpeed = 8.0f; // 全局呼吸频率（星星和拖尾共用）
    public float minScale = 0.6f;
    public float maxScale = 1.2f;
    [Range(0f, 1f)]
    public float minAlpha = 0.3f;
    [Range(0f, 1f)]
    public float maxAlpha = 1.0f;

    [Header("拖尾呼吸设置 (Trail Pulse)")]
    public float minTrailWidth = 0.4f; // 呼吸时拖尾的最细宽度
    public float maxTrailWidth = 1.2f; // 呼吸时拖尾的最粗宽度

    [Header("退场设置 (Destroy)")]
    public float lifeTime = 3.0f;
    public float fadeDuration = 1.0f;

    private SpriteRenderer sr;
    private TrailRenderer tr;
    private bool isFading = false;
    private float baseY;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        tr = GetComponent<TrailRenderer>();

        baseY = transform.position.y;
        StartCoroutine(FadeOutAndDestroy());
    }

    void Update()
    {
        // 1. 位移逻辑
        float newX = transform.position.x + (moveSpeed * Time.deltaTime);
        float newY = baseY + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(newX, newY, transform.position.z);

        // 2. 呼吸特效同步逻辑
        if (!isFading)
        {
            // 核心：生成 0 到 1 的律动进度值
            float pulseProgress = (Mathf.Sin(Time.time * flashSpeed) + 1f) / 2f;

            // A. 同步星星的大小
            float scale = Mathf.Lerp(minScale, maxScale, pulseProgress);
            transform.localScale = new Vector3(scale, scale, 1f);

            // B. 同步星星的透明度
            if (sr != null)
            {
                float currentAlpha = Mathf.Lerp(minAlpha, maxAlpha, pulseProgress);
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, currentAlpha);
            }

            // C. 同步整条五线谱拖尾的粗细（全新加入！）
            if (tr != null)
            {
                tr.widthMultiplier = Mathf.Lerp(minTrailWidth, maxTrailWidth, pulseProgress);
            }
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        isFading = true;

        float timer = 0f;
        Color startColor = sr.color;
        float startTrailWidth = tr != null ? tr.widthMultiplier : 1f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            if (sr != null)
                sr.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startColor.a, 0f, progress));

            if (tr != null)
                tr.widthMultiplier = Mathf.Lerp(startTrailWidth, 0f, progress);

            yield return null;
        }

        Destroy(gameObject);
    }
}