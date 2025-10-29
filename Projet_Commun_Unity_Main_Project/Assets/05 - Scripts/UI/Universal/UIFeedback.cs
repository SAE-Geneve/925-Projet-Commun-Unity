using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFeedback : MonoBehaviour
{
    [Tooltip("Curve of the height movement speed")]
    [SerializeField] private AnimationCurve heightCurve;
    
    private Image TemporaryVariableMaker(Image imageSample)
    {
        Image tempImage=Instantiate(imageSample, imageSample.transform.parent);
        tempImage.gameObject.SetActive(true);
        return tempImage;
    }
    public IEnumerator ImageFade(Image imageArg, float effectDuration)
    {
        var tempImage = TemporaryVariableMaker(imageArg);
        
        Color startcolor = tempImage.color;
        Color endcolor = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, 0);
        float t = 0.0f;
        while (tempImage.color.a > 0)
        {
            tempImage.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / effectDuration;
            }
            yield return null;
        }
        Destroy(tempImage.gameObject);
        yield return null;
    }
    
    public IEnumerator ImageUpFade(Image imageArg, float effectDuration)
    {
        var tempImage = TemporaryVariableMaker(imageArg);
        
        Color startcolor = tempImage.color;
        Color endcolor = new Color(tempImage.color.r, tempImage.color.g, tempImage.color.b, 0);
        float t = 0.0f;
        while (tempImage.color.a > 0)
        {
            tempImage.rectTransform.anchoredPosition += new Vector2(0f, 0.1f) * heightCurve.Evaluate(t);
            tempImage.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / effectDuration;
            }
            yield return null;
        }
        
        Destroy(tempImage.gameObject);
        yield return null;
    }
}
