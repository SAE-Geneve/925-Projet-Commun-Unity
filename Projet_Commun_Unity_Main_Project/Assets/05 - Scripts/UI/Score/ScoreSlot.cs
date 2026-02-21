using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreSlot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI missionScoreTmp;
    [SerializeField] private TextMeshProUGUI totalScoreTmp;

    [Header("Parameters")] 
    [SerializeField] private float scoreFillTime = 0.01f;

    private int totalScoreDisplay;
    private int missionScoreDisplay;

    public void SetMissionScore(int score)
    {
        missionScoreDisplay = score;
        missionScoreTmp.SetText(missionScoreDisplay.ToString());
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
            missionScoreTmp.SetText(missionScoreDisplay.ToString());
            
            yield return new WaitForSeconds(scoreFillTime);
        }
    }
}
