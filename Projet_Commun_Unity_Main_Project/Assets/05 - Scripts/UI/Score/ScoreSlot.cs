using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSlot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI missionScoreTmp;
    [SerializeField] private TextMeshProUGUI totalScoreTmp;
    [SerializeField] private TextMeshProUGUI playerNumberTmp;
    [SerializeField] private Image scoreBackdrop;

    [Header("Parameters")] 
    [SerializeField] private float scoreFillTime = 0.01f;
    [SerializeField] private Color disabledColor;

    private int totalScoreDisplay;
    private int missionScoreDisplay;

    public void SetMissionScore(int score)
    {
        missionScoreDisplay = score;
        if(missionScoreTmp)
        {
            missionScoreTmp.SetText(missionScoreDisplay.ToString());
        }
    }

    public void SetTotalScore(int score)
    {
        totalScoreDisplay = score;
        if(totalScoreTmp) totalScoreTmp.SetText(totalScoreDisplay.ToString());
    }

    public void DisabledAppearance()
    {
        missionScoreTmp.color = disabledColor;
        scoreBackdrop.color = disabledColor;
        playerNumberTmp.color = disabledColor;
    }

    public IEnumerator ScoreFillRoutine()
    {
        float duration = 3f;
        float elapsed = 0f;
    
        int startMission = missionScoreDisplay;
        int startTotal = totalScoreDisplay;
    
        int targetMission = 0;
        int targetTotal = totalScoreDisplay + missionScoreDisplay;
    
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
    
            missionScoreDisplay = Mathf.RoundToInt(Mathf.Lerp(startMission, targetMission, t));
            totalScoreDisplay = Mathf.RoundToInt(Mathf.Lerp(startTotal, targetTotal, t));
    
            totalScoreTmp.SetText(totalScoreDisplay.ToString());
    
            if (missionScoreTmp)
                missionScoreTmp.SetText(missionScoreDisplay.ToString());
    
            yield return null;
        }
    
        // Ensure exact final values
        missionScoreDisplay = targetMission;
        totalScoreDisplay = targetTotal;
    
        totalScoreTmp.SetText(totalScoreDisplay.ToString());
        if (missionScoreTmp)
            missionScoreTmp.SetText(missionScoreDisplay.ToString());
    }
}