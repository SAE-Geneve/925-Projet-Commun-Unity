using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AndTask : GameTask
{
    [SerializeField] private List<GameTask> tasks = new();
    
    protected override void Start()
    {
        base.Start();
        Subscribe();
    }
    
    private void Subscribe()
    {
        foreach (var task in tasks)
            task.OnSucceedAction += CheckTasks;
    }

    private void Unsubscribe()
    {
        foreach (var task in tasks)
            task.OnSucceedAction -= CheckTasks;
    }

    private void CheckTasks()
    {
        if (tasks.Any(task => !task.Done)) return;

        Succeed();
        Unsubscribe();
    }

    public override void ResetTask()
    {
        base.ResetTask();
        foreach (var task in tasks)
            task.ResetTask();
        
        Subscribe();
    }
}