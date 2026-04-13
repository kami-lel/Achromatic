using System.Collections;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;

public class Enemy: MonoBehaviour {
    // Public API  ################################################################

    public void Perish(Hit hit) {
        if (is_dead)
            return;

        is_dead = true;

        StopAllCoroutines();
        StartCoroutine(CoDeathAnimation());
    }

    // Inspector Fields  ########################################################

    [Header("Death Timing")]
    [SerializeField]
    private float punchDuration = 0.08f;

    [SerializeField]
    private float spinDuration = 0.35f;

    [SerializeField]
    private float fadeDuration = 0.25f;

    [Header("Death Scale")]
    [SerializeField]
    private float punchScale = 1.55f; // spike size

    [SerializeField]
    private float spinShrinkScale = 0.0f; // shrink target

    [Header("Death Spin")]
    [SerializeField]
    private float spinDegrees = 540f; // total rotation

    [Header("Hit Flash")]
    [SerializeField]
    private Color flashColor = Color.white;

    // private members  #########################################################

    private bool is_dead;
    private Vector3 original_scale;
    private SpriteRenderer[] renderers;

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        original_scale = transform.localScale;
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnEnable() {
        // reset per-spawn state for pool reuse
        is_dead = false;
        transform.localScale = original_scale;
        transform.eulerAngles = Vector3.zero;
        SetAlpha(1f);
        SetTint(Color.white);
    }

    // Death Sequence  =========================================================

    private IEnumerator CoDeathAnimation() {
        yield return StartCoroutine(CoPunch());
        yield return StartCoroutine(CoSpinShrink());
        yield return StartCoroutine(CoFadeOut());

        gameObject.SetActive(false);
    }

    // punch  ----------------------------------------------8------------------

    private IEnumerator CoPunch() {
        float t = 0f;
        SetTint(flashColor); // flash on hit

        while (t < punchDuration) {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / punchDuration);

            float s = Mathf.SmoothStep(1f, punchScale, p);
            transform.localScale = original_scale * s;

            yield return null;
        }

        SetTint(Color.white); // restore tint
    }

    // spin & shrink  ----------------------------------------------------------

    private IEnumerator CoSpinShrink() {
        float start_rot = transform.eulerAngles.z;
        float t = 0f;

        while (t < spinDuration) {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / spinDuration);
            float eased = Mathf.SmoothStep(0f, 1f, p);

            transform.eulerAngles = new Vector3(
                0f,
                0f,
                start_rot + Mathf.Lerp(0f, spinDegrees, eased)
            );

            transform.localScale =
                original_scale * Mathf.Lerp(1f, spinShrinkScale, eased);

            yield return null;
        }
    }

    // fade out  ---------------------------------------------------------------

    private IEnumerator CoFadeOut() {
        float t = 0f;

        while (t < fadeDuration) {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / fadeDuration);

            SetAlpha(Mathf.Lerp(1f, 0f, p));
            yield return null;
        }

        SetAlpha(0f); // guarantee fully transparent
    }

    // helpers  ================================================================

    private void SetAlpha(float alpha) {
        foreach (var r in renderers) {
            Color c = r.color;
            c.a = alpha;
            r.color = c;
        }
    }

    private void SetTint(Color tint) {
        foreach (var r in renderers)
            r.color = tint;
    }
}
