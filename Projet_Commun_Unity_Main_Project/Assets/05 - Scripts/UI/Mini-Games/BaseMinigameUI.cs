using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BaseMinigameUI : MonoBehaviour
{
    
    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI totalScore;
    private int _totalScore;
    [SerializeField] private TextMeshProUGUI totalSubScore1;
    private int _subScore1;
    [SerializeField] private TextMeshProUGUI totalSubScore2;
    private int _subScore2;
    
    [Header("Total Score Effects")]
    [SerializeField] private Image scoreImageFade;
    [SerializeField] private TextMeshProUGUI scoreTextFade;
    
    [Header("Secondary Score Effects")]
    [SerializeField] private TextMeshProUGUI subScoreEffect1;
    [SerializeField] private TextMeshProUGUI subScoreEffect2;
    
    
    private UITextEffects _uiTextEffects;
    
    private void Start()
    {
        if (TryGetComponent(out _uiTextEffects))
        {
            Debug.Log("Found UI Text Effects");
        }
    }
    
    public void TotalScoreIncrease()
    {
        StartCoroutine(UniversalUIFeedback.DoImageFade(scoreImageFade));
        StartCoroutine(_uiTextEffects.DoTextFadeMoveDown(scoreTextFade));
        
        _totalScore += 150;
        totalScore.text = ""+_totalScore.ToString("00000000");
    }
    
    public void SubScore1Increase()
    {
        StartCoroutine(_uiTextEffects.DoTextFade(subScoreEffect1));
        
        _subScore1 += 1;
        totalSubScore1.text = ""+_subScore1.ToString();
    }
    
    public void SubScore2Increase()
    {
        StartCoroutine(_uiTextEffects.DoTextFade(subScoreEffect2));
        
        _subScore2 += 1;
        totalSubScore2.text = ""+_subScore2.ToString();
    }
}
