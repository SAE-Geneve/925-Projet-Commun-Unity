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
    private int _totalScore;
    
    [SerializeField] private GameObject[] scorePanel;
    [SerializeField] private TextMeshProUGUI[] playerScoreText;
    [SerializeField] private TextMeshProUGUI[] subScoreEffectText;
    private int[] _playerScore;
    
    private ScoreManager _scoreManager;
    
    [Header("Total Score Effects")]
    [SerializeField] private Image scoreImageFade;
    [SerializeField] private TextMeshProUGUI scoreTextFade;
    
    private UIScreenEffects _uiScreenEffects;
    
    private void Start()
    {
        _scoreManager = GameManager.Instance.Scores;

        _playerScore = new int[4];
        for (int i = 0; i < 4; i++)
        {
            _playerScore[i] = 0;
        }
        if (TryGetComponent(out _uiScreenEffects))
        {
            Debug.Log("Found UI Text Effects");
            if (scoreImageFade != null)
            {
                _uiScreenEffects.ImagePoolCreation(scoreImageFade);
            }
            else
            {
                Debug.LogWarning($"[{name}] NewMinigameUI : 'scoreImageFade' est manquant !");
            }
        }
        else
        {
            Debug.LogWarning($"[{name}] NewMinigameUI : Composant 'UIScreenEffects' manquant sur cet objet !");
        }
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
        for (int id = 0; id < PlayerManager.Instance.Players.Count; id++)
        {
            if (_playerScore[id] != _scoreManager.MissionScores[id])
            {
                PlayerScoreIncrease(id, _scoreManager.MissionScores[id]);        
                TotalScoreIncrease(_scoreManager.MissionScores[id]);
            }
        }
    }
    
    private void TotalScoreIncrease(int score)
    {
        if (_uiScreenEffects != null)
        {
            StartCoroutine(_uiScreenEffects.DoImagePoolFade());
            
            if (scoreTextFade != null)
            {
                if(_totalScore < score)
                {
                    scoreTextFade.text = "+" + (score-_totalScore);
                }
                else if (_totalScore > score)
                {
                    scoreTextFade.text = "-" + (_totalScore-score);
                }
                StartCoroutine(_uiScreenEffects.DoTextFadeMoveDown(scoreTextFade));
            }
        }
        
        if (totalScore != null)
        {
            totalScore.text = _scoreManager.TotalMissionScore().ToString("00000000");
        }
        _totalScore = score;
    }
    
    private void PlayerScoreIncrease(int id, int score)
    {
        playerScoreText[id].text = score.ToString("00000");
        
        if (_playerScore[id] < score)
        {
            subScoreEffectText[id].text = "+"+(score-_playerScore[id]);
        }
        else if (_playerScore[id] > score)
        {
            subScoreEffectText[id].text = "-"+(_playerScore[id]-score);
        }
        
        StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffectText[id]));
        _playerScore[id] = score;
    }
}