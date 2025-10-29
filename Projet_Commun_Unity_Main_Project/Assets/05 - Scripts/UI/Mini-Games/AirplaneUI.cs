using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AirplaneUI : MonoBehaviour
{
    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI totalScore;
    private int _totalScore;
    
    [Header("Client Satisfaction")]
    [SerializeField] private Slider clientSatisfactionSlider;
    private float _clientScore=0.5f;
    
    [Header("Total Score Effects")]
    [SerializeField] private Image scoreImageFade;
    [SerializeField] private TextMeshProUGUI scoreTextFade;
    
    private UIScreenEffects _uiScreenEffects;
    
    private void Start()
    {
        if (TryGetComponent(out _uiScreenEffects))
        {
            Debug.Log("Found UI Text Effects");
        }
    }
    
    public void TotalScoreIncrease()
    {
        StartCoroutine(_uiScreenEffects.DoImageFade(scoreImageFade));
        StartCoroutine(_uiScreenEffects.DoTextFadeMoveDown(scoreTextFade));
        
        _totalScore += 150;
        totalScore.text = ""+_totalScore.ToString("00000000");
    }
    
    public void IncreaseSatisfaction()
    {
        if (_clientScore < 1f)
        {
            _clientScore += 0.05f;
        }
        clientSatisfactionSlider.value = _clientScore;
    }
    
    public void DecreaseSatisfaction()
    {
        if (_clientScore > 0f)
        {
            _clientScore -= 0.05f;
        }
        Debug.Log(_clientScore);
        clientSatisfactionSlider.value = _clientScore;
    }
}
