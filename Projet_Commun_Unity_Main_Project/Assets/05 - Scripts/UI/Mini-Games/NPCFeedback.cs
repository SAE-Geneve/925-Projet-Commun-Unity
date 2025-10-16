using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPCFeedback : MonoBehaviour
{
    [SerializeField] private float effectDuration=2.5f;
    
    [Header("Reaction Images")]
    [SerializeField] private Image happyImage;
    [SerializeField] private Image unhappyImage;
    
    public void HappyResult()
    {
        happyImage.gameObject.SetActive(true);
        StartCoroutine(ImageFade(happyImage));
    }
    public void UnhappyResult()
    {
        unhappyImage.gameObject.SetActive(true);
        StartCoroutine(ImageFade(unhappyImage));
    }
    
    private IEnumerator ImageFade(Image imageToFade)
    {
        Color startcolor = imageToFade.color;
        Color endcolor = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 0);
        float t = 0.0f;
        while (imageToFade.color.a > 0)
        {
            imageToFade.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / effectDuration;
            }
            yield return null;
        }
        imageToFade.gameObject.SetActive(false);
        imageToFade.color = startcolor;
        yield return null;
    }
}
