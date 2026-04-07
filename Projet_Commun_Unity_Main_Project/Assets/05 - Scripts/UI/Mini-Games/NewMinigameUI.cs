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
    [SerializeField] private TextMeshProUGUI[] playerScoreText;
    
    [Header("Panels")]
    [SerializeField] private GameObject[] scorePanel;
    [SerializeField] private GameObject timerPanel;
    
    [Header("Effects")]
    [SerializeField] private Image scoreImageFade;
    [SerializeField] private TextMeshProUGUI scoreTextFade;
    [SerializeField] private TextMeshProUGUI[] subScoreEffectText;
    
    private int[] _playerScore;
    private int _totalScore;
    
    private Canvas _canvas;
    private ScoreManager _scoreManager;
    private UIScreenEffects _uiScreenEffects;
    private PlayerManager _playerManager;
    
    private void Start()
    {
        _scoreManager = GameManager.Instance.Scores;
        _playerManager = PlayerManager.Instance;

        _playerScore = new int[4];
        for (int i = 0; i < 4; i++)
        {
            _playerScore[i] = 0;
        }

        if (TryGetComponent(out _canvas))
        {
            Debug.Log("Found canvas");
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

    public void StartMinigame()
    {
        _canvas.enabled = true;
        timerPanel.SetActive(true);
        for (int i = 0; i < _playerManager.Players.Count; i++)
        {
            scorePanel[i].SetActive(true);
        }
    }

    public void EndMinigame()
    {
        _canvas.enabled = false;
        timerPanel.SetActive(false);
        for (int i = 0; i < _playerManager.Players.Count; i++)
        {
            playerScoreText[i].text = 0.ToString("0000$");
            scorePanel[i].SetActive(false);
            _playerScore[i] = 0;
        }
        totalScore.text = 0.ToString("0000000$");
        
    }

    private void Update()
    {
        if(GameManager.Instance.Context==GameContext.Mission)
        {
            for (int id = 0; id < _playerManager.Players.Count; id++)
            {
                if (_playerScore[id] != _scoreManager.MissionScores[id])
                {
                    PlayerScoreIncrease(id, _scoreManager.MissionScores[id]);
                    TotalScoreIncrease(_scoreManager.MissionScores[id]);
                }
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
            totalScore.text = _scoreManager.TotalMissionScore().ToString("0000000$");
        }
        _totalScore = score;
    }
    
    private void PlayerScoreIncrease(int id, int score)
    {
        playerScoreText[id].text = score.ToString("0000$");
        
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