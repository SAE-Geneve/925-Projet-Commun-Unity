using System.Collections.Generic;
using UnityEngine;

public class OrTask : GameTask
{
    [SerializeField] private List<GameTask> tasks = new List<GameTask>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        foreach (var task in tasks)
        {
            task.OnSucceedAction += SucceedTasks;
        }
    }

    private void SucceedTasks()
    { 
        Succeed();
        
        foreach (var task in tasks)
        {
            task.OnSucceedAction -= SucceedTasks;
        }
    }
}
