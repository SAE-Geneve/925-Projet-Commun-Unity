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
    [SerializeField] private Color[] rankColors;
    
    [Header("Progress Bar")]
    [SerializeField] private Image progressBarFill;

    [Header("Settings")]
    [SerializeField] private float fillSpeed = 0.5f;

    [Header("Rank Up Feedback")]
    [SerializeField] private float bounceScale = 1.5f;
    [SerializeField] private float bounceDuration = 0.5f;

    private int _lastRankIndex;
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

        _lastRankIndex = _gameManager.CurrentRankIndex;

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
        
        // Sécurise l'index pour ne jamais dépasser la taille des tableaux
        int displayRankIndex = Mathf.Min(_gameManager.CurrentRankIndex, _gameManager.Ranks.Length - 1);
        int colorIndex = Mathf.Min(_gameManager.CurrentRankIndex, rankColors.Length - 1);

        moneyText.SetText($"Money {_scoreManager.TotalScore}$");
        rankText.SetText($"Rank - {_gameManager.Ranks[displayRankIndex].rankName}");
        
        if (rankColors != null && rankColors.Length > 0)
        {
            rankText.color = rankColors[colorIndex];
            progressBarFill.color = rankColors[colorIndex];
        }

        if (_gameManager.CurrentRankIndex > _lastRankIndex)
        {
            TriggerRankUpFeedback();
            _lastRankIndex = _gameManager.CurrentRankIndex;
        }

        if (_gameManager.CurrentRankIndex >= _gameManager.Ranks.Length)
        {
            progressText.SetText("MAX !");
            _targetFillRatio = 1f;
        }
        else
        {
            _targetFillRatio = Mathf.Clamp01((float)_scoreManager.TotalScore / _gameManager.Ranks[displayRankIndex].pointObjectif);
            progressText.SetText($"{_scoreManager.TotalScore}$ / { _gameManager.Ranks[displayRankIndex].pointObjectif}$");
        }
    }
    
    private void TriggerRankUpFeedback()
    {
        if (AudioManager.Instance != null && AudioManager.Instance.rankUpSFX != null)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.rankUpSFX);
        }

        if (rankText != null)
        {
            StartCoroutine(BounceAnimation(rankText.transform));
        }
    }

    private System.Collections.IEnumerator BounceAnimation(Transform target)
    {
        Vector3 originalScale = Vector3.one;
        float halfDuration = bounceDuration / 2f;
        float timer = 0f;

        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            target.localScale = Vector3.Lerp(originalScale, originalScale * bounceScale, timer / halfDuration);
            yield return null;
        }

        timer = 0f;
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;
            target.localScale = Vector3.Lerp(originalScale * bounceScale, originalScale, timer / halfDuration);
            yield return null;
        }

        target.localScale = originalScale;
    }
}