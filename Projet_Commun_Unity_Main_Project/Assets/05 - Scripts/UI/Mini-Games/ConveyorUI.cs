using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConveyorUI : MonoBehaviour
{
    
    [Header("Score Texts")]
    [SerializeField] TextMeshProUGUI totalScore=null;
    [SerializeField] TextMeshProUGUI totalCollected=null;
    [SerializeField] TextMeshProUGUI totalUnclaimed=null;
    
    [Header("Score Effects")]
    [SerializeField] Image scoreImageFade=null;
    [SerializeField] TextMeshProUGUI scoreTextFade=null;
    
    public void ScoreEffect()
    {
        StartCoroutine(UniversalUIFeedback.DoFade(scoreImageFade));
        StartCoroutine(UniversalUIFeedback.DoTextFade(scoreTextFade));
    }
}
