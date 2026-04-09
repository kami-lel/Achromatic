using System.Collections;
using Assets._Achromatic.Scripts.Scores;
using UnityEngine;

public class Enemy: MonoBehaviour {

    // Public API  #############################################################

    public void Perish(Hit hit) {
        if (is_dead)
            return;
        is_dead = true;

        StopAllCoroutines();
        StartCoroutine(CoDeathAnimation());
    }

    // Inspector Fields  #######################################################

    [Header("Death Timing")]
    [SerializeField] private float punchDuration = 0.08f;
    [SerializeField] private float spinDuration = 0.35f;
    [SerializeField] private float fadeDuration = 0.25f;

    [Header("Death Scale")]
    [SerializeField] private float punchScale = 1.55f;  // spike size
    [SerializeField] private float spinShrinkScale = 0.0f;  // shrink target

    [Header("Death Spin")]
    [SerializeField] private float spinDegrees = 540f;   // total rotation

    [Header("Hit Flash")]
    [SerializeField] private Color flashColor = Color.white;

    // private members  ########################################################

    private bool is_dead = false;
    private Vector3 original_scale;
    private SpriteRenderer[] renderers;

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        original_scale = transform.localScale;
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    // Death Sequence  =========================================================

    private IEnumerator CoDeathAnimation() {
        yield return StartCoroutine(CoPunch());
        yield return StartCoroutine(CoSpinShrink());
        yield return StartCoroutine(CoFadeOut());

        Destroy(gameObject);
    }

    // punch  ------------------------------------------------------------------

    private IEnumerator CoPunch() {
        float t = 0f;

        SetFlash(flashColor);  // flash white on hit

        while (t < punchDuration) {
            t += Time.deltaTime;
            float s = Mathf.SmoothStep(1f, punchScale, t / punchDuration);
            transform.localScale = original_scale * s;
            yield return null;
        }

        SetFlash(Color.white);  // restore tint
    }

    // spin & shrink  ----------------------------------------------------------

    private IEnumerator CoSpinShrink() {
        float start_rot = transform.eulerAngles.z;
        float t = 0f;

        while (t < spinDuration) {
            t += Time.deltaTime;
            float eased = Mathf.SmoothStep(0f, 1f, t / spinDuration);

            float angle = Mathf.Lerp(0f, spinDegrees, eased);
            transform.eulerAngles = new Vector3(
                0f, 0f, start_rot + angle
            );

            float s = Mathf.Lerp(1f, spinShrinkScale, eased);
            transform.localScale = original_scale * s;

            yield return null;
        }
    }

    // fade out  ---------------------------------------------------------------

    private IEnumerator CoFadeOut() {
        float t = 0f;

        while (t < fadeDuration) {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(1f, 0f, t / fadeDuration));
            yield return null;
        }

        SetAlpha(0f);
    }

    // helpers  ================================================================

    private void SetAlpha(float alpha) {
        foreach (var r in renderers) {
            Color c = r.color;
            c.a = alpha;
            r.color = c;
        }
    }

    private void SetFlash(Color tint) {
        foreach (var r in renderers)
            r.color = tint;
    }

}