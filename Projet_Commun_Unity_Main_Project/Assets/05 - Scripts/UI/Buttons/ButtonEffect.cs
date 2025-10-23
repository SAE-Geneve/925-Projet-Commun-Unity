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
    
    
    private UITextEffects _uiTextEffects;

    private void Start()
    {
        if (TryGetComponent(out _uiTextEffects))
        {
            Debug.Log("Found UI Text Effects");
        }
    }
    public void ScoreEffect()
    {
        StartCoroutine(UniversalUIFeedback.DoImageFade(scoreEffect));
        StartCoroutine(_uiTextEffects.DoTextFadeMoveDown(scoreEffectText));
        _score += 150;
        totalScore.text = ""+_score.ToString("00000000");
    }
}
