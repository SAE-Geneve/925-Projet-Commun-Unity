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
    
    
    private UIScreenEffects _uiScreenEffects;

    private void Start()
    {
        if (TryGetComponent(out _uiScreenEffects))
        {
            Debug.Log("Found UI Text Effects");
        }
    }
    public void ScoreEffect()
    {
        StartCoroutine(_uiScreenEffects.DoImageFade(scoreEffect));
        StartCoroutine(_uiScreenEffects.DoTextFadeMoveDown(scoreEffectText));
        _score += 150;
        totalScore.text = ""+_score.ToString("00000000");
    }
}
