using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankProgressUI : MonoBehaviour
{
    // [System.Serializable]
    // public struct RankData
    // {
    //     public string rankName;    // Ex: "Bronze", "Argent", "Or"
    //     public int moneyRequired;  // Ex: 100, 500, 1000
    // }

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI progressText;

    [Header("Progress Bar")]
    [SerializeField] private Image progressBarFill; // L'image orange qui doit se remplir

    [Header("Settings")]
    [SerializeField] private float fillSpeed = 0.5f; // 0.5 = met 2 secondes pour se remplir de 0% à 100%

    private float _targetFillRatio;

    // [Header("Rank Configuration")]
    // [SerializeField] private RankData[] ranks; // Liste de tous tes rangs

    // private int _currentRankIndex = 0;
    private ScoreManager _scoreManager;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _scoreManager = _gameManager.Scores;
        
        if (_scoreManager != null)
        {
            // On s'abonne à l'événement de mise à jour du score
            _scoreManager.OnTotalScoreUpdated += UpdateUI;
        }

        // Première mise à jour au lancement
        UpdateUI();

        // Au démarrage, on initialise la jauge visuelle directement à la bonne taille
        if (progressBarFill != null) progressBarFill.fillAmount = _targetFillRatio;
    }

    private void OnDestroy()
    {
        if (_scoreManager != null)
        {
            // On se désabonne quand l'objet est détruit
            _scoreManager.OnTotalScoreUpdated -= UpdateUI;
        }
    }

    private void Update()
    {
        // Remplit visuellement la barre à chaque frame jusqu'à atteindre la cible
        if (progressBarFill != null && progressBarFill.fillAmount != _targetFillRatio)
        {
            // Mathf.MoveTowards avance à une vitesse constante (fillSpeed)
            progressBarFill.fillAmount = Mathf.MoveTowards(progressBarFill.fillAmount, _targetFillRatio, fillSpeed * Time.deltaTime);
        }
    }

    private void UpdateUI()
    {
        if (_gameManager.Ranks == null || _gameManager.Ranks.Length == 0) return;

        // // 1. Calculer le score total
        // int totalMoney = 0;
        // if (_scoreManager != null)
        // {
        //     for (int i = 0; i < PlayerManager.Instance.PlayerCount; i++)
        //     {
        //         totalMoney += _scoreManager.PlayerScores[i];
        //     }
        // }

        // // 2. Vérifier si on passe au rang supérieur
        // CheckRankUp(totalMoney);

        // // 3. Mettre à jour l'interface
        // RankData currentRank = ranks[_currentRankIndex];
        
        moneyText.SetText($"Money {_scoreManager.TotalScore}$");
        rankText.SetText($"Rank - {_gameManager.Ranks[_gameManager.CurrentRankIndex].rankName}");

        _targetFillRatio = Mathf.Clamp01((float)_scoreManager.TotalScore / _gameManager.Ranks[_gameManager.CurrentRankIndex].pointObjectif);

        // Si on est au rang max, on peut afficher un texte différent (optionnel)
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

    // private void CheckRankUp(int currentMoney)
    // {
    //     while (_currentRankIndex < ranks.Length - 1 && currentMoney >= ranks[_currentRankIndex].moneyRequired)
    //     {
    //         _currentRankIndex++;
    //     }
    // }
}