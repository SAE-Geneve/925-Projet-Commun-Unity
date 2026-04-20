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

    [Header("Rank Up Feedback")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip rankUpClip;
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
        
        
        moneyText.SetText($"Money {_scoreManager.TotalScore}$");
        rankText.SetText($"Rank - {_gameManager.Ranks[_gameManager.CurrentRankIndex].rankName}");

        if (_gameManager.CurrentRankIndex > _lastRankIndex)
        {
            TriggerRankUpFeedback();
            _lastRankIndex = _gameManager.CurrentRankIndex;
        }

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
    
    private void TriggerRankUpFeedback()
    {
        if (audioSource != null && rankUpClip != null)
        {
            audioSource.PlayOneShot(rankUpClip);
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