using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private int scoreToAddOrSub = 10;
    [SerializeField] private float timeToAddOrSub = 10f;
    
    public void UnlockMissions()
    {
        MissionManager missionManager = FindAnyObjectByType<MissionManager>();
        
        if (missionManager)
        {
            missionManager.UnlockAllMissions();
            Debug.LogWarning("All missions are unlocked");
        }
    }

    public void AddMissionScore() => AddScore(scoreToAddOrSub, false);
    public void AddGlobalScore() => AddScore(scoreToAddOrSub, true);
    public void SubMissionScore() => SubScore(scoreToAddOrSub, false);
    public void SubGlobalScore() => SubScore(scoreToAddOrSub, true);

    private void AddScore(int score, bool total)
    {
        ScoreManager scoreManager = GameManager.Instance.Scores;

        if (scoreManager == null)
        {
            Debug.LogWarning("Score Manager is null, cannot add score");
            return;
        }

        if (!total && !GameManager.Instance.CurrentMission)
        {
            Debug.LogWarning("Not in a current mission, cannot modify mission score");
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            if(total) scoreManager.AddPlayerScore(score, i);
            else scoreManager.AddPlayerScore(score, i);
        }
    }

    private void SubScore(int score, bool total)
    {
        ScoreManager scoreManager = GameManager.Instance.Scores;
        
        if (scoreManager == null)
        {
            Debug.LogWarning("Score Manager is null, cannot sub score");
            return;
        }
        
        if (!total && !GameManager.Instance.CurrentMission)
        {
            Debug.LogWarning("Not in a current mission, cannot modify mission score");
            return;
        }
        
        for (int i = 0; i < 4; i++)
        {
            if(total) scoreManager.SubPlayerScore(score, i);
            else scoreManager.SubPlayerScore(score, i);
        }
    }
    
    public void AddTime() => GameManager.Instance.Timer += timeToAddOrSub;
    public void SubTime() => GameManager.Instance.Timer -= timeToAddOrSub;
    public void GameOver() => GameManager.Instance.Timer = 0;
}
