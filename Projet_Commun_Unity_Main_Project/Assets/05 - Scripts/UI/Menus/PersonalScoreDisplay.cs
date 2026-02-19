using System;
using TMPro;
using UnityEngine;

public class PersonalScoreDisplay : MonoBehaviour
{
    [SerializeField] private GameObject[] scoreDisplay;
    [SerializeField] private TextMeshProUGUI[] scoreText;
    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = GameManager.Instance.Scores;
        for (int i = 0; i < PlayerManager.Instance.PlayerCount; i++)
        {
            scoreDisplay[i].SetActive(true);
        }
    }

    void OnEnable()
    {
        for (int i = 0; i < PlayerManager.Instance.PlayerCount; i++)
        {
            scoreText[i].SetText($"{scoreManager.TotalScores[i]:00000}");
        }
    }
}
