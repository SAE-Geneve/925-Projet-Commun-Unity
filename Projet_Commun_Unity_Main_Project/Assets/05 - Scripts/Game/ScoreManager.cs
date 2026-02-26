using UnityEngine;

public class ScoreManager
{
    public readonly int[] TotalScores = new int[4];
    public readonly int[] MissionScores = new int[4];

    public void FillTotalScores()
    {
        for (int i = 0; i < MissionScores.Length; i++)
        {
            AddTotalScore(MissionScores[i], i);
            MissionScores[i] = 0;
        }
        Debug.LogWarning("Score Reset");
    }
    
    public int TotalGameScore()
    {
        int totalGameScore = 0;
        for (int i = 0; i < TotalScores.Length; i++)
        {
            totalGameScore += TotalScores[i];
        }
        return totalGameScore;
    }

    public int TotalMissionScore()
    {
        int totalMissionScore = 0;
        for (int i = 0; i < MissionScores.Length; i++)
        {
            totalMissionScore += MissionScores[i];
        }
        return totalMissionScore;
    }
    private void AddTotalScore(int score, int id)
    {
        TotalScores[id] += score;
        Debug.Log("added " + score + " to " + id + " total");
    }
    
    public void AddMissionScore(int score, int id)
    {
        MissionScores[id] += score;
        Debug.Log("added " + score + " to " + id + " mission total");
    }
    
    public void SubMissionScore(int score, int id)
    {
        MissionScores[id] -= score;
        Debug.Log("removed " + score + " to " + id + " mission total");
    }

}
