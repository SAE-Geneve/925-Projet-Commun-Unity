using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalScore;
    private int _score;
    
    [Header("Effect Images")]
    [SerializeField] Image scoreEffect;
    [SerializeField] TextMeshProUGUI scoreEffectText;

    public void ScoreEffect()
    {
        StartCoroutine(UniversalUIFeedback.DoFade(scoreEffect));
        StartCoroutine(UniversalUIFeedback.DoTextFade(scoreEffectText));
        _score += 150;
        totalScore.text = ""+_score.ToString("00000000");
    }
}
