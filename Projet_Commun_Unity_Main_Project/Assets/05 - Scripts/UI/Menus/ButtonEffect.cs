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
        StartCoroutine(UniversalUIFeedback.DoFade(scoreEffect));
        StartCoroutine(UniversalUIFeedback.DoTextFade(scoreEffectText));
        score += 150;
        totalScore.text = ""+score.ToString("00000000");
    }
}
