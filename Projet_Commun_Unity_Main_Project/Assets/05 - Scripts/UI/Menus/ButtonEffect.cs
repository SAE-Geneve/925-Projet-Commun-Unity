using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalScore=null;
    private int score = 0;
    
    [Header("Effect Images")]
    [SerializeField] Image scoreEffect=null;
    [SerializeField] TextMeshProUGUI scoreEffectText=null;

    public void ScoreEffect()
    {
        StartCoroutine(DoFade(scoreEffect));
        StartCoroutine(DoTextFade(scoreEffectText));
        score += 150;
        totalScore.text = ""+score.ToString("00000000");
    }
    
    public IEnumerator DoFade(Image fadeImage)
    {
        fadeImage.gameObject.SetActive(true);
        Color startcolor = fadeImage.color;
        Color endcolor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        float t = 0.0f;
        float duration = 1.5f;
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
    
    public IEnumerator DoTextFade(TextMeshProUGUI fadeText)
    {
        fadeText.gameObject.SetActive(true);
        Color startcolor = fadeText.color;
        Color endcolor = new Color(fadeText.color.r, fadeText.color.g, fadeText.color.b, 0);
        float t = 0.0f;
        float duration = 1.5f;
        while (fadeText.color.a > 0)
        {
            fadeText.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / duration;
            }
            yield return null;
        }
        fadeText.gameObject.SetActive(false);
        fadeText.color = startcolor;
        yield return null;
    }
}
