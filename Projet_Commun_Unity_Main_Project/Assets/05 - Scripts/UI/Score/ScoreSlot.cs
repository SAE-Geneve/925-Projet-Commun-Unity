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

    public void DisabledAppearance()
    {
        missionScoreTmp.color = disabledColor;
        scoreBackdrop.color = disabledColor;
        playerNumberTmp.color = disabledColor;
    }

    public IEnumerator ScoreFillRoutine()
    {
        while (missionScoreDisplay != 0)
        {
            if (missionScoreDisplay > 0)
            {
                totalScoreDisplay++;
                missionScoreDisplay--;
            }
            else
            {
                totalScoreDisplay--;
                missionScoreDisplay++;
            }

            totalScoreTmp.SetText(totalScoreDisplay.ToString());
            if(missionScoreTmp)
            {
                missionScoreTmp.SetText(missionScoreDisplay.ToString());
            }
            
            yield return new WaitForSeconds(scoreFillTime);
        }
    }
}
