using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadingBlockingPanel: MonoBehaviour {

    // Public Methods  #########################################################
    public void FadeOut(System.Action onComplete = null) =>
            StartCoroutine(CoFade(0f, 1f, onComplete));
    public void FadeIn(System.Action onComplete = null) =>
            StartCoroutine(CoFade(1f, 0f, onComplete));

    // Inspector Fields  #######################################################

    [SerializeField]
    private float fadingDurationSec = 1.5f;

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        panelImage = GetComponent<Image>();

        panelImage.color = new Color(0f, 0f, 0f, 1f); // solid black
    }

    // private members  ########################################################
    // cached references
    private Image panelImage;

    // private methods  ########################################################

    private IEnumerator CoFade(
            float from,
            float to,
            System.Action onComplete = null) {
        float duration = Mathf.Max(1e-5f, fadingDurationSec); // ensure safe
        float t = 0f;

        Color color = panelImage.color;
        color.a = from;
        panelImage.color = color;

        while (t < duration) {
            t += Time.deltaTime;
            color.a = Mathf.SmoothStep(from, to, t / duration);  // ease-in-out
            panelImage.color = color;
            yield return null;
        }

        color.a = to;
        panelImage.color = color;

        onComplete?.Invoke();
    }

}

