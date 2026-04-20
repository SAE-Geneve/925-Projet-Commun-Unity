using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankProgressUI : MonoBehaviour
{
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI progressText;

    [Header("Progress Bar")]
    [SerializeField] private Image progressBarFill;

    [Header("Settings")]
    [SerializeField] private float fillSpeed = 0.5f;

    private float _targetFillRatio;

    private ScoreManager _scoreManager;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _scoreManager = _gameManager.Scores;
        
        if (_scoreManager != null)
        {
            _scoreManager.OnTotalScoreUpdated += UpdateUI;
        }

        UpdateUI();

        if (progressBarFill != null) progressBarFill.fillAmount = _targetFillRatio;
    }

    private void OnDestroy()
    {
        if (_scoreManager != null)
        {
            _scoreManager.OnTotalScoreUpdated -= UpdateUI;
        }
    }

    private void Update()
    {
        if (progressBarFill != null && progressBarFill.fillAmount != _targetFillRatio)
        {
            progressBarFill.fillAmount = Mathf.MoveTowards(progressBarFill.fillAmount, _targetFillRatio, fillSpeed * Time.deltaTime);
        }
    }

    private void UpdateUI()
    {
        if (_gameManager.Ranks == null || _gameManager.Ranks.Length == 0) return;
        
        
        moneyText.SetText($"Money {_scoreManager.TotalScore}$");
        rankText.SetText($"Rank - {_gameManager.Ranks[_gameManager.CurrentRankIndex].rankName}");

        _targetFillRatio = Mathf.Clamp01((float)_scoreManager.TotalScore / _gameManager.Ranks[_gameManager.CurrentRankIndex].pointObjectif);

        if (_gameManager.CurrentRankIndex >= _gameManager.Ranks.Length - 1 && _scoreManager.TotalScore >= _gameManager.Ranks[_gameManager.CurrentRankIndex].pointObjectif)
        {
            progressText.SetText("MAX !");
            _targetFillRatio = 1f;
        }
        else
        {
            progressText.SetText($"{_scoreManager.TotalScore}$ / { _gameManager.Ranks[_gameManager.CurrentRankIndex].pointObjectif}$");
        }
    }
    
}