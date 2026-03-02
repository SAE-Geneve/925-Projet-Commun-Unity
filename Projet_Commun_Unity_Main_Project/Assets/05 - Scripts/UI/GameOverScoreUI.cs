using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScoreUI : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Image[] scoreBox;
    [SerializeField] private TextMeshProUGUI[] scoresTmp;
    [SerializeField] private Color[] scoreBoxColors;
    [SerializeField] private Color[] textColors;
    [SerializeField] private TextMeshProUGUI totalScoreTmp;
    
    private void Start()
    {
        ScoreManager scoreManager = GameManager.Instance.Scores;
        
        for (int i = 0; i < PlayerManager.Instance.PlayerCount; i++)
        {
            scoreBox[i].enabled = true;
            scoresTmp[i].enabled = true;
            scoresTmp[i].SetText(scoreManager.TotalScores[i].ToString());
            scoresTmp[i].color = textColors[i];
            scoreBox[i].color = scoreBoxColors[i];
        }
        totalScoreTmp.SetText(scoreManager.TotalGameScore().ToString("000000"));
    }
}
