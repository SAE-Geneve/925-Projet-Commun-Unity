using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConveyorUI : MonoBehaviour
{
    
    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI totalScore;
    private int _totalScore;
    [SerializeField] private TextMeshProUGUI totalCollected;
    private int _collectedScore;
    [SerializeField] private TextMeshProUGUI totalUnclaimed;
    private int _unclaimedScore;
    
    [Header("Total Score Effects")]
    [SerializeField] private Image scoreImageFade;
    [SerializeField] private TextMeshProUGUI scoreTextFade;
    
    [Header("Secondary Score Effects")]
    [SerializeField] private TextMeshProUGUI collectedScoreText;
    [SerializeField] private TextMeshProUGUI unclaimedScoreText;
    
    public void TotalScoreIncrease()
    {
        StartCoroutine(UniversalUIFeedback.DoFade(scoreImageFade));
        StartCoroutine(UniversalUIFeedback.DoTextFade(scoreTextFade));
        StartCoroutine(UniversalUIFeedback.DoTextMoveDown(scoreTextFade));
        _totalScore += 150;
        totalScore.text = ""+_totalScore.ToString("00000000");
    }
    
    public void CollectedScoreIncrease()
    {
        StartCoroutine(UniversalUIFeedback.DoTextFade(collectedScoreText));
        _collectedScore += 1;
        totalCollected.text = ""+_collectedScore.ToString();
    }
    
    public void UnclaimedScoreIncrease()
    {
        StartCoroutine(UniversalUIFeedback.DoTextFade(unclaimedScoreText));
        _unclaimedScore += 1;
        totalUnclaimed.text = ""+_unclaimedScore.ToString();
    }
    
    //TO BE USED WHEN BUTTONS AREN'T USED ANYMORE
    public void SecondaryScoreIncrease(int score, TextMeshProUGUI scoreText, TextMeshProUGUI effectText)
    {
        StartCoroutine(UniversalUIFeedback.DoTextFade(effectText));
        score += 1;
        scoreText.text = ""+score.ToString();
    }
}
