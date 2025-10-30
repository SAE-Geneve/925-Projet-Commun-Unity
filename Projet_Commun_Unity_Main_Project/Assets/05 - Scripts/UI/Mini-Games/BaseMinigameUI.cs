using TMPro;
using UnityEditor;
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
    
    public void TotalScoreIncrease()
    {
        StartCoroutine(_uiScreenEffects.DoImageFade());
        StartCoroutine(_uiScreenEffects.DoTextFadeMoveDown(scoreTextFade));
        
        _totalScore += 150;
        totalScore.text = ""+_totalScore.ToString("00000000");
    }
    
    public void SubScore1Increase()
    {
        StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect1));
        
        _subScore1 += 1;
        totalSubScore1.text = ""+_subScore1.ToString();
    }
    
    public void SubScore2Increase()
    {
        StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect2));
        
        _subScore2 += 1;
        totalSubScore2.text = ""+_subScore2.ToString();
    }
    
    public void SubScore3Increase()
    {
        StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect3));
        
        _subScore3 += 1;
        totalSubScore3.text = ""+_subScore3.ToString();
    }
    
    public void SubScore4Increase()
    {
        StartCoroutine(_uiScreenEffects.DoTextFade(subScoreEffect4));
        
        _subScore4 += 1;
        totalSubScore4.text = ""+_subScore4.ToString();
    }
}
