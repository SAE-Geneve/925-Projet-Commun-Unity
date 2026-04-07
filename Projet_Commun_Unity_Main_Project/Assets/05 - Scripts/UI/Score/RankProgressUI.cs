using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RankProgressUI : MonoBehaviour
{
    [System.Serializable]
    public struct RankData
    {
        public string rankName;    // Ex: "Bronze", "Argent", "Or"
        public int moneyRequired;  // Ex: 100, 500, 1000
    }

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI progressText;
    [SerializeField] private Image progressBarFill;

    [Header("Rank Configuration")]
    [SerializeField] private RankData[] ranks; // Liste de tous tes rangs

    private int _currentRankIndex = 0;
    private ScoreManager _scoreManager;

    private void Start()
    {
        _scoreManager = GameManager.Instance.Scores;
        
        if (_scoreManager != null)
        {
            // On s'abonne à l'événement de mise à jour du score
            _scoreManager.OnScoreUpdated += UpdateUI;
        }

        // Première mise à jour au lancement
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (_scoreManager != null)
        {
            // On se désabonne quand l'objet est détruit
            _scoreManager.OnScoreUpdated -= UpdateUI;
        }
    }

    private void UpdateUI()
    {
        if (ranks == null || ranks.Length == 0) return;

        // 1. Calculer le score total
        int totalMoney = 0;
        if (_scoreManager != null)
        {
            for (int i = 0; i < PlayerManager.Instance.PlayerCount; i++)
            {
                totalMoney += _scoreManager.PlayerScores[i];
            }
        }

        // 2. Vérifier si on passe au rang supérieur
        CheckRankUp(totalMoney);

        // 3. Mettre à jour l'interface
        RankData currentRank = ranks[_currentRankIndex];
        
        moneyText.SetText($"Money {totalMoney}$");
        rankText.SetText($"Rank - {currentRank.rankName}");

        float fillRatio = Mathf.Clamp01((float)totalMoney / currentRank.moneyRequired);
        progressBarFill.fillAmount = fillRatio;

        // Si on est au rang max, on peut afficher un texte différent (optionnel)
        if (_currentRankIndex >= ranks.Length - 1 && totalMoney >= currentRank.moneyRequired)
        {
            progressText.SetText("MAX !");
            progressBarFill.fillAmount = 1f;
        }
        else
        {
            progressText.SetText($"{totalMoney}$ / {currentRank.moneyRequired}$");
        }
    }

    private void CheckRankUp(int currentMoney)
    {
        while (_currentRankIndex < ranks.Length - 1 && currentMoney >= ranks[_currentRankIndex].moneyRequired)
        {
            _currentRankIndex++;
        }
    }
}