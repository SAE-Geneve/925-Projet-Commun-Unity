using System;
using System.Linq;
using UnityEngine;

public class ScoreManager
{
    public readonly int[] PlayerScores = new int[4];
    public readonly int[] MissionScores = new int[4];

    private int _totalScore;
    public int TotalScore => _totalScore;
    public event Action OnScoreUpdated;
    public event Action OnTotalScoreUpdated;

    // public void FillTotalScores()
    // {
    //     for (int i = 0; i < MissionScores.Length; i++)
    //     {
    //         AddPlayerScore(MissionScores[i], i);
    //         MissionScores[i] = 0;
    //     }
    //     Debug.Log("Score Reset");
    //     OnScoreUpdated?.Invoke();
    // }
    
    //public int TotalGameScore() => TotalScores.Sum();

    public int TotalMissionScore() => MissionScores.Sum();

    private void AddTotalScore(int score)
    {
        _totalScore += score;
        Debug.Log("added " + score + " to total score");
        OnTotalScoreUpdated?.Invoke();
    }
    
    private void SubTotalScore(int score)
    {
        _totalScore -= score;
        Debug.Log("removed " + score + " from total score");
        OnTotalScoreUpdated?.Invoke();
    }
    
    // public void AddPlayerScore(int score, int id)
    // {
    //     PlayerScores[id] += score;
    //     Debug.Log("added " + score + " to " + id + " player score");
    //     OnScoreUpdated?.Invoke();
    //
    //     if (score > 0)
    //     {
    //         AddTotalScore(score);
    //     }
    // }
    //
    // public void SubPlayerScore(int score, int id)
    // {
    //     PlayerScores[id] -= score;
    //     Debug.Log("removed " + score + " to " + id + "player score");
    //     OnScoreUpdated?.Invoke();
    //     
    //     if (score > 0)
    //     {
    //         SubTotalScore(score);
    //     }
    // }
    
    public void AddPlayerScore(int score, int id)
    {
        PlayerScores[id] += score;
        Debug.Log("added " + score + " to " + id + " player score");
        OnScoreUpdated?.Invoke();
        
        if (score > 0)
        {
            AddTotalScore(score);
        }
        
        if (GameManager.Instance.CurrentMission != null)
        {
            MissionScores[id] += score;
            Debug.Log("added " + score + " to " + id + " mission score");
            OnScoreUpdated?.Invoke();
        }
    }
    
    public void SubPlayerScore(int score, int id)
    {
        PlayerScores[id] -= score;
        Debug.Log("removed " + score + " to " + id + "player score");
        OnScoreUpdated?.Invoke();
        
        if (score > 0)
        {
            SubTotalScore(score);
        }

        if (GameManager.Instance.CurrentMission != null)
        {
            MissionScores[id] -= score;
            Debug.Log("removed " + score + " to " + id + " mission score");
            OnScoreUpdated?.Invoke();
        }
    }
    
    public void SubOnlyPlayerScore(int score, int id)
    {
        PlayerScores[id] -= score;
        Debug.Log("removed " + score + " to " + id + "player score");
        OnScoreUpdated?.Invoke();
    }

    public void ResetMissionScores()
    {
        for (int i = 0; i < MissionScores.Length; i++)
        {
            MissionScores[i] = 0;
        }
    }
}