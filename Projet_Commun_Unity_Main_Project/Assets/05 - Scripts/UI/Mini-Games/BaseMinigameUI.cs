using System;
using TMPro;
using UnityEngine;
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
    
    [Header("Secondary Score Image Effects")]
    [SerializeField] private Image subScoreImage1;
    [SerializeField] private Image subScoreImage2;
    [SerializeField] private Image subScoreImage3;
    [SerializeField] private Image subScoreImage4;
    
    [Header("Secondary Score Text Effects")]
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
            if (scoreImageFade != null)
            {
                _uiScreenEffects.ImagePoolCreation(scoreImageFade);
            }
            else
            {
                Debug.LogWarning($"[{name}] BaseMinigameUI : 'scoreImageFade' est manquant !");
            }
        }
        else
        {
            Debug.LogWarning($"[{name}] BaseMinigameUI : Composant 'UIScreenEffects' manquant sur cet objet !");
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
        _totalScore = ScoreSystem.TotalMinigameScore;
        
        if (_uiScreenEffects != null)
        {
            StartCoroutine(_uiScreenEffects.DoImagePoolFade());
            
            if (scoreTextFade != null)
                StartCoroutine(_uiScreenEffects.DoTextFadeMoveDown(scoreTextFade));
        }
        
        if (totalScore != null)
        {
            totalScore.text = _totalScore.ToString("00000000");
        }
    }
    
    private void SubScore1Increase()
    {
        _subScore1 = ScoreSystem.Subscore1;

        if (_uiScreenEffects != null && subScoreEffect1 != null)
            StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect1));
        if (subScoreImage1 != null)
            StartCoroutine(_uiScreenEffects.DoImageFade(subScoreImage1));
        if (subScoreImage2 != null)
            StartCoroutine(_uiScreenEffects.DoImageFade(subScoreImage2));
        
        if (totalSubScore1 != null)
            totalSubScore1.text = _subScore1.ToString();
    }
    
    private void SubScore2Increase()
    {
        _subScore2 = ScoreSystem.Subscore2;

        if (_uiScreenEffects != null && subScoreEffect2 != null)
            StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect2));
        if (subScoreImage2 != null)
            StartCoroutine(_uiScreenEffects.DoImageFade(subScoreImage2));
        
        if (totalSubScore2 != null)
            totalSubScore2.text = _subScore2.ToString();
    }
    
    private void SubScore3Increase()
    {
        _subScore3 = ScoreSystem.Subscore3;

        if (_uiScreenEffects != null && subScoreEffect3 != null)
            StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect3));
        
        
        if (subScoreImage3 != null)
            StartCoroutine(_uiScreenEffects.DoImageFade(subScoreImage3));
        if (subScoreImage4 != null)
            StartCoroutine(_uiScreenEffects.DoImageFade(subScoreImage4));
        
        if (totalSubScore3 != null)
            totalSubScore3.text = _subScore3.ToString();
    }
    
    private void SubScore4Increase()
    {
        _subScore4 = ScoreSystem.Subscore4;

        if (_uiScreenEffects != null && subScoreEffect4 != null)
            StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect4));
        if (subScoreImage4 != null)
            StartCoroutine(_uiScreenEffects.DoImageFade(subScoreImage4));
        
        if (totalSubScore4 != null)
            totalSubScore4.text = _subScore4.ToString();
    }
}