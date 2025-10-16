using System.Collections;
using TMPro;
using UnityEngine;

public class UITextEffects : MonoBehaviour
{
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
        float duration = 1.5f;
        while (tempText.color.a > 0)
        {
            tempText.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / duration;
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
        float duration = 1.5f;
        
        while (t < duration)
        { 
            tempText.rectTransform.anchoredPosition += new Vector2(0f, -50f) * Time.deltaTime;
            t += Time.deltaTime;
            yield return null;
        }
        
        Destroy(tempText.gameObject);
        yield return null;
    }
    
    public IEnumerator DoTextFadeMoveDown(TextMeshProUGUI fadeText)
    {
        var tempText = TemporaryVariableMaker(fadeText);
        
        Vector3 startPos = tempText.transform.localPosition;
        float t = 0.0f;
        float duration = 1.5f;
        Color startcolor = tempText.color;
        Color endcolor = new Color(tempText.color.r, tempText.color.g, tempText.color.b, 0);
        
        while(tempText.color.a > 0)
        { 
            tempText.transform.localPosition += new Vector3(0f, -50f, 0f)*Time.deltaTime;
            tempText.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / duration;
            }
            yield return null;
        }
        
        Destroy(tempText.gameObject);
        yield return null;
    }
}
