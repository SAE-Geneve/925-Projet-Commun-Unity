using System.Linq;
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
        Debug.Log("Score Reset");
    }
    
    public int TotalGameScore() => TotalScores.Sum();

    public int TotalMissionScore() => MissionScores.Sum();
    
    public void AddTotalScore(int score, int id)
    {
        TotalScores[id] += score;
        Debug.Log("added " + score + " to " + id + " total score");
    }

    public void SubTotalScore(int score, int id)
    {
        TotalScores[id] -= score;
        Debug.Log("removed " + score + " to " + id + "total score");
    }
    
    public void AddMissionScore(int score, int id)
    {
        MissionScores[id] += score;
        Debug.Log("added " + score + " to " + id + " mission score");
    }
    
    public void SubMissionScore(int score, int id)
    {
        MissionScores[id] -= score;
        Debug.Log("removed " + score + " to " + id + " mission score");
    }
}