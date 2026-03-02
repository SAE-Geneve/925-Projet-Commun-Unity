using System;
using TMPro;
using UnityEngine;

public class PersonalScoreDisplay : MonoBehaviour
{
    [SerializeField] private GameObject[] scoreDisplay;
    [SerializeField] private TextMeshProUGUI[] scoreText;
    private ScoreManager _scoreManager;

    private void Start()
    {
        _scoreManager = GameManager.Instance.Scores;
        
        for (int i = 0; i < PlayerManager.Instance.PlayerCount; i++)
        {
            scoreDisplay[i].SetActive(true);
        }
        
        if (_scoreManager != null)
        {
            _scoreManager.OnScoreUpdated += UpdateScoreUI;
        }
        
        UpdateScoreUI();
    }
    
    private void UpdateScoreUI()
    {
        if (_scoreManager == null) return;
        
        for (int i = 0; i < PlayerManager.Instance.PlayerCount; i++)
        {
            scoreText[i].SetText($"{_scoreManager.TotalScores[i]:00000}");
        }
    }
    
    private void OnDestroy()
    {
        if (_scoreManager != null)
        {
            _scoreManager.OnScoreUpdated -= UpdateScoreUI;
        }
    }
    
}