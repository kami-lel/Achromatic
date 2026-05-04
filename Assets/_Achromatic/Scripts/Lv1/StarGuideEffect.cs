using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SpriteRenderer), typeof(TrailRenderer))]
public class StarGuideEffect : MonoBehaviour
{
    [Header("移动与浮动 (Movement & Float)")]
    public float moveSpeed = 8.0f;
    public float floatSpeed = 3.0f;
    public float floatHeight = 0.5f;

    [Header("星星呼吸设置 (Star Pulse)")]
    public float flashSpeed = 8.0f;
    public float minScale = 0.6f;
    public float maxScale = 1.2f;
    [Range(0f, 1f)]
    public float minAlpha = 0.3f;
    [Range(0f, 1f)]
    public float maxAlpha = 1.0f;

    [Header("主拖尾呼吸 (Main Trail Pulse)")]
    public float minTrailWidth = 0.2f;
    public float maxTrailWidth = 0.6f;

    [Header("缠绕拖尾设置 (Wavy Trails - 仿例图核心)")]
    public float waveAmplitude = 0.25f; // 波浪起伏的幅度（多宽）
    public float waveFrequency = 12.0f; // 波浪交织的频率（多密）
    public float wavyTrailWidth = 0.1f; // 缠绕细线的宽度（建议比主拖尾细）

    [Header("退场设置 (Destroy)")]
    public float lifeTime = 3.0f;
    public float fadeDuration = 1.0f;

    private SpriteRenderer sr;
    private TrailRenderer mainTr;
    private List<TrailRenderer> allTrails = new List<TrailRenderer>(); // 存放所有拖尾
    private Transform[] wavyEmitters; // 波浪发射器

    private bool isFading = false;
    private float baseY;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        mainTr = GetComponent<TrailRenderer>();
        allTrails.Add(mainTr);
        baseY = transform.position.y;

        // --- 全自动生成“交织波浪”发射器 ---
        wavyEmitters = new Transform[2];
        for (int i = 0; i < 2; i++)
        {
            // 创建隐藏的子物体
            GameObject emitterObj = new GameObject("WavyEmitter_" + i);
            emitterObj.transform.SetParent(this.transform);
            emitterObj.transform.localPosition = Vector3.zero;
            wavyEmitters[i] = emitterObj.transform;

            // 复制主拖尾的组件属性，确保材质和颜色一致
            TrailRenderer childTr = emitterObj.AddComponent<TrailRenderer>();
            childTr.sharedMaterial = mainTr.sharedMaterial;
            childTr.time = mainTr.time;
            childTr.colorGradient = mainTr.colorGradient;

            // 设置细线宽度
            childTr.startWidth = wavyTrailWidth;
            childTr.endWidth = 0f;

            allTrails.Add(childTr);
        }

        StartCoroutine(FadeOutAndDestroy());
    }

    void Update()
    {
        // 1. 整体向右推进 + 整体上下缓慢漂浮
        float newX = transform.position.x + (moveSpeed * Time.deltaTime);
        float newY = baseY + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(newX, newY, transform.position.z);

        if (!isFading)
        {
            // 2. 呼吸律动 (大小 + 透明度 + 主拖尾宽度)
            float pulseProgress = (Mathf.Sin(Time.time * flashSpeed) + 1f) / 2f;
            float scale = Mathf.Lerp(minScale, maxScale, pulseProgress);
            transform.localScale = new Vector3(scale, scale, 1f);

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(minAlpha, maxAlpha, pulseProgress));
            mainTr.widthMultiplier = Mathf.Lerp(minTrailWidth, maxTrailWidth, pulseProgress);

            // 3. 核心：让两根细线产生正弦波交织！
            // 利用相差 180 度 (Mathf.PI) 的相位，画出完美的 DNA 螺旋结构
            float wave1 = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
            float wave2 = Mathf.Sin(Time.time * waveFrequency + Mathf.PI) * waveAmplitude;

            wavyEmitters[0].localPosition = new Vector3(0, wave1, 0);
            wavyEmitters[1].localPosition = new Vector3(0, wave2, 0);
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        yield return new WaitForSeconds(lifeTime);
        isFading = true;

        float timer = 0f;
        Color startColor = sr.color;

        // 记录所有拖尾当前的宽度
        float[] startWidths = new float[allTrails.Count];
        for (int i = 0; i < allTrails.Count; i++) startWidths[i] = allTrails[i].widthMultiplier;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;

            // 星星本体淡出
            sr.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startColor.a, 0f, progress));

            // 所有拖尾（主干+交织线）平滑收束到 0
            for (int i = 0; i < allTrails.Count; i++)
            {
                if (allTrails[i] != null)
                    allTrails[i].widthMultiplier = Mathf.Lerp(startWidths[i], 0f, progress);
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}