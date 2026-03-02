using System.Collections.Generic;
using UnityEngine;

public class Boarding : MonoBehaviour
{
    [SerializeField] private BoardingAITask aiTask;
    [SerializeField] private StackTask propTask;
    
    [SerializeField] private int[] aiTaskGoals = new int[4];
    [SerializeField] private int[] propTaskGoals = new int[4];
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AdaptMission();
    }

    public void AdaptMission()
    {
        switch (PlayerManager.Instance.PlayerCount)
        {
            case 1:
                aiTask.SetMultipleTaskLimit(aiTaskGoals[0]);
                propTask.SetGoal(propTaskGoals[0]);
                break;
            case 2:
                aiTask.SetMultipleTaskLimit(aiTaskGoals[1]);
                propTask.SetGoal(propTaskGoals[1]);
                break;
            case 3:
                aiTask.SetMultipleTaskLimit(aiTaskGoals[2]);
                propTask.SetGoal(propTaskGoals[2]);
                break;
            case 4:
                aiTask.SetMultipleTaskLimit(aiTaskGoals[3]);
                propTask.SetGoal(propTaskGoals[3]);
                break;
        }
    }
}
