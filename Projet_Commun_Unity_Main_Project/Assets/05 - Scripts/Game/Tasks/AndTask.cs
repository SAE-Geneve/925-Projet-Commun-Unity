using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AndTask : GameTask
{
    [SerializeField] private List<GameTask> tasks = new List<GameTask>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        foreach (var task in tasks)
        {
            task.OnSucceedAction += CheckTasks;
        }
    }

    private void CheckTasks()
    {
        if (tasks.Any(task => !task.Done))
        {
            return;
        }
        
        Succeed();

        foreach (var task in tasks)
        {
            task.OnSucceedAction -= CheckTasks;

        }
    }
}
