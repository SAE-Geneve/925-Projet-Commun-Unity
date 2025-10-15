using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConveyorUI : MonoBehaviour
{
    
    [Header("Score Texts")]
    [SerializeField] TextMeshProUGUI totalScore=null;
    private int total_score = 0;
    [SerializeField] TextMeshProUGUI totalCollected=null;
    private int collected_score = 0;
    [SerializeField] TextMeshProUGUI totalUnclaimed=null;
    private int unclaimed_score = 0;
    
    [Header("Total Score Effects")]
    [SerializeField] Image scoreImageFade=null;
    [SerializeField] TextMeshProUGUI scoreTextFade=null;
    
    [Header("Secondary Score Effects")]
    [SerializeField] TextMeshProUGUI collectedScoreText=null;
    [SerializeField] TextMeshProUGUI unclaimedScoreText=null;
    
    public void TotalScoreIncrease()
    {
        StartCoroutine(UniversalUIFeedback.DoFade(scoreImageFade));
        StartCoroutine(UniversalUIFeedback.DoTextFade(scoreTextFade));
        total_score += 150;
        totalScore.text = ""+total_score.ToString("00000000");
    }
    
    public void CollectedScoreIncrease()
    {
        StartCoroutine(UniversalUIFeedback.DoTextFade(collectedScoreText));
        collected_score += 1;
        totalCollected.text = ""+collected_score.ToString();
    }
    
    public void UnclaimedScoreIncrease()
    {
        StartCoroutine(UniversalUIFeedback.DoTextFade(unclaimedScoreText));
        unclaimed_score += 1;
        totalUnclaimed.text = ""+unclaimed_score.ToString();
    }
    
    //TO BE USED WHEN BUTTONS AREN'T USED ANYMORE
    public void SecondaryScoreIncrease(int score, TextMeshProUGUI scoreText, TextMeshProUGUI effectText)
    {
        StartCoroutine(UniversalUIFeedback.DoTextFade(effectText));
        score += 1;
        scoreText.text = ""+score.ToString();
    }
}
