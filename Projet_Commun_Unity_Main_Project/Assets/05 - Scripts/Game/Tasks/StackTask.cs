using TMPro;
using UnityEngine;

public class StackTask : TriggerTask
{
    [Header("Container")]
    [SerializeField] [Min(2)] private int stackGoal = 5;
    [SerializeField] private TextMeshProUGUI stackGoalTmp;

    private int _currentStack;
    
    protected override void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent(out Prop prop) || prop.Type != _propType) return;
        
        _currentStack++;
        Check();
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.TryGetComponent(out Prop prop) || prop.Type != _propType) return;
        
        _currentStack--;
        Check();
    }

    private void Check()
    {
        if (_currentStack >= stackGoal && !Done)
        {
            Succeed();
            stackGoalTmp.color = Color.green;
        }
        else if (_currentStack < stackGoal && Done)
        {
            Done = false;
            stackGoalTmp.color = Color.white;
        }
        
        stackGoalTmp.SetText($"{_currentStack}/{stackGoal}");
    }

    public override void ResetTask()
    {
        base.ResetTask();
        _currentStack = 0;
        
        stackGoalTmp.color = Color.white;
        stackGoalTmp.SetText($"{_currentStack}/{stackGoal}");
    }
}