using System.Collections;
using TMPro;
using UnityEngine;

public class UITextEffects : MonoBehaviour
{
    [SerializeField] private float effectDuration=2.5f;
    private TextMeshProUGUI TemporaryVariableMaker(TextMeshProUGUI textSample)
    {
        TextMeshProUGUI tempText=Instantiate(textSample, textSample.transform.parent);
        tempText.gameObject.SetActive(true);
        return tempText;
    }
    
    public IEnumerator DoTextFade(TextMeshProUGUI fadeText)
    {
        var tempText = TemporaryVariableMaker(fadeText);
        
        Color startcolor = tempText.color;
        Color endcolor = new Color(tempText.color.r, tempText.color.g, tempText.color.b, 0);
        float t = 0.0f;
        while (tempText.color.a > 0)
        {
            tempText.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / effectDuration;
            }
            yield return null;
        }
        Destroy(tempText.gameObject);
        yield return null;
    }
    
    public IEnumerator DoTextMoveDown(TextMeshProUGUI fadeText)
    {
        var tempText = TemporaryVariableMaker(fadeText);
        
        float t = 0.0f;
        
        while (t < effectDuration)
        { 
            tempText.rectTransform.anchoredPosition += new Vector2(0f, -25f) * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }
        
        Destroy(tempText.gameObject);
        yield return null;
    }
    
    public IEnumerator DoTextFadeMoveDown(TextMeshProUGUI fadeText)
    {
        var tempText = TemporaryVariableMaker(fadeText);
        
        float t = 0.0f;
        Color startcolor = tempText.color;
        Color endcolor = new Color(tempText.color.r, tempText.color.g, tempText.color.b, 0);
        
        while(tempText.color.a > 0)
        { 
            tempText.rectTransform.anchoredPosition += new Vector2(0f, -25f) * Time.deltaTime;
            tempText.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / effectDuration;
            }
            yield return null;
        }
        
        Destroy(tempText.gameObject);
        yield return null;
    }
}
