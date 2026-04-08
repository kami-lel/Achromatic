using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadingBlockingPanel: MonoBehaviour {

    // Public Methods  #########################################################
    public void FadeOut() => StartCoroutine(CoFade(0f, 1f));
    // HACK
    // public void FadeIn() => StartCoroutine(CoFade(1f, 0f));
    public void FadeIn() {
        StartCoroutine(CoFade(1f, 0f));
    }

    // Inspector Fields  #######################################################

    [SerializeField]
    private float fadingDurationSec = 1.5f;

    // MonoBehavior Lifecycle  #################################################

    private void Awake() {
        panelImage = GetComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 1f); // transparent black
        Debug.Log("panel awake");  // HACK
    }

    // private members  ########################################################
    // cached references
    private Image panelImage;

    // private methods  ########################################################

    private IEnumerator CoFade(float from, float to) {
        float duration = Mathf.Max(1e-5f, fadingDurationSec); // ensure safe
        float t = 0f;

        Color color = panelImage.color;
        color.a = from;
        panelImage.color = color;

        while (t < duration) {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(from, to, t / duration);
            Debug.Log(color);  // HACK
            panelImage.color = color;
            yield return null;
        }

        color.a = to;
        panelImage.color = color;
    }

}

