using System.Collections;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadingBlockingPanel: MonoBehaviour {

    // Public Methods  #########################################################
    public void FadeOut() => StartCoroutine(CoFade(0f, 1f));

    public void FadeIn() => StartCoroutine(CoFade(1f, 0f));

    // Inspector Fields  #######################################################

    [SerializeField]
    private float fadingDurationSec = 1.5f;

    // private members  ########################################################

    private IEnumerator CoFade(float from, float to) {
        float t = 0f;
        Color color = panelImage.color;
        color.a = from;
        panelImage.color = color;

        while (t < fadingDurationSec) {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(from, to, t / fadingDurationSec);
            panelImage.color = color;
            yield return null;
        }
        color.a = to;
        panelImage.color = color;
    }

    private void Awake() {
        panelImage = GetComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0f); // transparent black
    }

    // private members  ########################################################
    // cached references
    private Image panelImage;
}

