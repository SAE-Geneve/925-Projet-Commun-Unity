using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFeedback : MonoBehaviour
{
    private Image TemporaryVariableMaker(Image imageSample)
    {
        Image tempImage=Instantiate(imageSample, imageSample.transform.parent);
        tempImage.gameObject.SetActive(true);
        return tempImage;
    }
    public IEnumerator ImageFade(Image imageArg, float effectDuration)
    {
        var imageToFade = TemporaryVariableMaker(imageArg);
        
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
