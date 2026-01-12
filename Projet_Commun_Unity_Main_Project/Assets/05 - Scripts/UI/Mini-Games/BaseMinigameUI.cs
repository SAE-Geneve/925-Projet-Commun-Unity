using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class BaseMinigameUI : MonoBehaviour
{
    //public bool ExtraScores;
    
    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI totalScore;
    private int _totalScore;
    [SerializeField] private TextMeshProUGUI totalSubScore1;
    private int _subScore1;
    [SerializeField] private TextMeshProUGUI totalSubScore2;
    private int _subScore2;
    [SerializeField] public TMP_Text totalSubScore3;
    private int _subScore3;
    [SerializeField] public TMP_Text totalSubScore4;
    private int _subScore4;
    
    [Header("Total Score Effects")]
    [SerializeField] private Image scoreImageFade;
    [SerializeField] private TextMeshProUGUI scoreTextFade;
    
    [Header("Secondary Score Effects")]
    [SerializeField] private TextMeshProUGUI subScoreEffect1;
    [SerializeField] private TextMeshProUGUI subScoreEffect2;
    [SerializeField] public TextMeshProUGUI subScoreEffect3;
    [SerializeField] public TextMeshProUGUI subScoreEffect4;
    
    private UIScreenEffects _uiScreenEffects;
    
    private void Start()
    {
        if (TryGetComponent(out _uiScreenEffects))
        {
            Debug.Log("Found UI Text Effects");
            _uiScreenEffects.ImagePoolCreation(scoreImageFade);
        }
    }

    private void Update()
    {
        if (_subScore1 != ScoreSystem.Subscore1)
        {
            SubScore1Increase();
        }
        else if (_subScore2 != ScoreSystem.Subscore2)
        {
            SubScore2Increase();
        }
        else if (_subScore3 != ScoreSystem.Subscore3)
        {
            SubScore3Increase();
        }
        else if (_subScore4 != ScoreSystem.Subscore4)
        {
            SubScore4Increase();
        }
        if (_totalScore != ScoreSystem.TotalMinigameScore)
        {
            TotalScoreIncrease();
        }
    }
    
    private void TotalScoreIncrease()
    {
        StartCoroutine(_uiScreenEffects.DoImagePoolFade());
        StartCoroutine(_uiScreenEffects.DoTextFadeMoveDown(scoreTextFade));
        
        _totalScore = ScoreSystem.TotalMinigameScore;
        totalScore.text = ""+_totalScore.ToString("00000000");
    }
    
    private void SubScore1Increase()
    {
        StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect1));
        
        _subScore1 = ScoreSystem.Subscore1;
        totalSubScore1.text = ""+ScoreSystem.Subscore1;
    }
    
    private void SubScore2Increase()
    {
        StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect2));
        
        _subScore2 = ScoreSystem.Subscore2;
        totalSubScore2.text = ""+ScoreSystem.Subscore2;
    }
    
    private void SubScore3Increase()
    {
        StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect3));
        
        _subScore3 = ScoreSystem.Subscore3;
        totalSubScore3.text = ""+ScoreSystem.Subscore3;
    }
    
    private void SubScore4Increase()
    {
        StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect4));
        
        _subScore4 = ScoreSystem.Subscore4;
        totalSubScore4.text = ""+ScoreSystem.Subscore4;
    }
}
