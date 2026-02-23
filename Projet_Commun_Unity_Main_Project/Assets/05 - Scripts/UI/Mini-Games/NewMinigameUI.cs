using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NewMinigameUI : MonoBehaviour
{
    //public bool ExtraScores;
    
    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI totalScore;
    
    [SerializeField] private GameObject[] scorePanel;
    [SerializeField] private TextMeshProUGUI[] playerScoreText;
    private int[] _playerScore;
    
    private ScoreManager _scoreManager;
    
    [Header("Total Score Effects")]
    [SerializeField] private Image scoreImageFade;
    [SerializeField] private TextMeshProUGUI scoreTextFade;
    
    [Header("Secondary Score Image Effects")]
    [SerializeField] private Image leftSubScoreImage;
    [SerializeField] private Image rightSubscoreImage;
    
    [Header("Secondary Score Text Effects")]
    [SerializeField] private TextMeshProUGUI subScoreEffect1;
    [SerializeField] private TextMeshProUGUI subScoreEffect2;
    [SerializeField] public TextMeshProUGUI subScoreEffect3;
    [SerializeField] public TextMeshProUGUI subScoreEffect4;
    
    private UIScreenEffects _uiScreenEffects;
    
    private void Start()
    {
        _scoreManager = GameManager.Instance.Scores;

        _playerScore = new int[4];
        for (int i = 0; i < 4; i++)
        {
            _playerScore[i] = 0;
        }
        // if (TryGetComponent(out _uiScreenEffects))
        // {
        //     Debug.Log("Found UI Text Effects");
        //     if (scoreImageFade != null)
        //     {
        //         _uiScreenEffects.ImagePoolCreation(scoreImageFade);
        //     }
        //     else
        //     {
        //         Debug.LogWarning($"[{name}] BaseMinigameUI : 'scoreImageFade' est manquant !");
        //     }
        // }
        // else
        // {
        //     Debug.LogWarning($"[{name}] BaseMinigameUI : Composant 'UIScreenEffects' manquant sur cet objet !");
        // }
    }

    private void OnEnable()
    {
        for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
        {
            scorePanel[i].SetActive(true);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < PlayerManager.Instance.Players.Count; i++)
        {
            scorePanel[i].SetActive(false);
        }
    }

    private void Update()
    {
        for (int x = 0; x < PlayerManager.Instance.Players.Count; x++)
        {
            if (_playerScore[x] != _scoreManager.MissionScores[x])
            {
                PlayerScoreIncrease(x, _scoreManager.MissionScores[x]);        
                TotalScoreIncrease(_scoreManager.MissionScores[x]);
            }
        }
    }
    
    private void TotalScoreIncrease(int score)
    {
        // if (_uiScreenEffects != null)
        // {
        //     StartCoroutine(_uiScreenEffects.DoImagePoolFade());
        //     
        //     if (scoreTextFade != null)
        //         StartCoroutine(_uiScreenEffects.DoTextFadeMoveDown(scoreTextFade));
        // }
        
        if (totalScore != null)
        {
            totalScore.text = score.ToString("00000000");
        }
    }
    
    private void PlayerScoreIncrease(int id, int score)
    {
        _playerScore[id] = score;
        playerScoreText[id].text = score.ToString("00000");
        // if (_uiScreenEffects != null && subScoreEffect1 != null)
        //     StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect1));
        // if (leftSubScoreImage != null)
        //     StartCoroutine(_uiScreenEffects.DoImageFade(leftSubScoreImage));
    }
}