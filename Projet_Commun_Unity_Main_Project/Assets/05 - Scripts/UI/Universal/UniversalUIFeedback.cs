using System.Collections;
using UnityEngine;
using UnityEngine.UI;

//Class for UI elements that doesn't DESTROY or CREATE new UI elements and that are NON-STACKABLE
public class UniversalUIFeedback
{
    //Maybe make it stackable instead...
    public static IEnumerator DoImageFade(Image fadeImage)
    {
        fadeImage.gameObject.SetActive(true);
        Color startcolor = fadeImage.color;
        Color endcolor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        float t = 0.0f;
        float duration = 2.5f;
        while (fadeImage.color.a > 0)
        {
            fadeImage.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / duration;
            }
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
        fadeImage.color = startcolor;
        yield return null;
    }
    
}
