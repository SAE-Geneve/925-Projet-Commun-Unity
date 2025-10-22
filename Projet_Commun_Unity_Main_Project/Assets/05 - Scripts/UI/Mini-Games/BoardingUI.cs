using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BoardingUI : MonoBehaviour
{
    
    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI totalScore;
    private int _totalScore;
    [SerializeField] private TextMeshProUGUI totalPassengers;
    private int _passengersScore;
    [SerializeField] private TextMeshProUGUI totalLuggages;
    private int _luggagesScore;
    
    [Header("Total Score Effects")]
    [SerializeField] private Image scoreImageFade;
    [SerializeField] private TextMeshProUGUI scoreTextFade;
    
    [Header("Secondary Score Effects")]
    [SerializeField] private TextMeshProUGUI passengersScoreText;
    [SerializeField] private TextMeshProUGUI luggagesScoreText;
    
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
    
    public void PassengersBoardedIncrease()
    {
        StartCoroutine(_uiTextEffects.DoTextFade(passengersScoreText));
        
        _passengersScore += 1;
        totalPassengers.text = ""+_passengersScore.ToString();
    }
    
    public void LuggagesStowedIncreased()
    {
        StartCoroutine(_uiTextEffects.DoTextFade(luggagesScoreText));
        
        _luggagesScore += 1;
        totalLuggages.text = ""+_luggagesScore.ToString();
    }
}
