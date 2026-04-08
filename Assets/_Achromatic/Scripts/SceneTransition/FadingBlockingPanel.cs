using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadingBlockingPanel: MonoBehaviour {

    // Public Methods  #########################################################
    //
    public void FadeOut() => StartCoroutine(CoFade(0f, 1f));

    public void FadeIn() => StartCoroutine(CoFade(1f, 0f));

    // Inspector Fields  #######################################################

    [SerializeField]
    private float fadingDurationSec = 1.5f;

    // private members  ########################################################
    private IEnumerator CoFade(float from, float to) {
        float t = 0f;
        Color c = panelImage.color;
        c.a = from;
        panelImage.color = c;

        while (t < fadingDurationSec) {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / fadingDurationSec);
            panelImage.color = c;
            yield return null;
        }
        c.a = to;
        panelImage.color = c;
    }



    // private members  ########################################################
    // cached references
    private Image panelImage;
}
