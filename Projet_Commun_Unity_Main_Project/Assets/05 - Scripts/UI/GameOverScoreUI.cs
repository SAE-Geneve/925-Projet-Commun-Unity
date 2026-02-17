using TMPro;
using UnityEngine;

public class GameOverScoreUI : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private TextMeshProUGUI[] scoresTmp;
    
    private void Start()
    {
        ScoreManager scoreManager = GameManager.Instance.Scores;
        
        for (int i = 0; i < PlayerManager.Instance.PlayerCount; i++)
        {
            scoresTmp[i].gameObject.SetActive(true);
            scoresTmp[i].SetText(scoreManager.TotalScores[i].ToString());
        }
    }
}
