using System.Collections.Generic;
using UnityEngine;

public class ConveyorBreakdownEvent : GameEvent
{
    [Header("Conveyor Settings")]
    [SerializeField] private List<GameTask> _restartButtonTasks; 
    [SerializeField] private List<ConveyorBelt> _conveyorBelts;

    private bool _isConveyorBroken = false;

    private void Start()
    {
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) buttonTask.OnSucceedAction += HandleButtonPressed;
        }
    }

    public override bool IsEventActive()
    {
        // Si le tapis est déjà en panne, le Manager ne pourra pas repiocher cet événement
        return _isConveyorBroken;
    }

    public override void TriggerEvent()
    {
        Debug.Log("EVENT: Le tapis roulant tombe en panne !");
        _isConveyorBroken = true;

        foreach (var belt in _conveyorBelts)
        {
            if(belt != null) belt.StopBelt();
        }

        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) 
            {
                buttonTask.ResetTask();
                if (buttonTask.TryGetComponent<Animator>(out Animator anim)) anim.SetBool("IsBroken", true); 
            }
        }
    }

    public override void ResetEvent()
    {
        _isConveyorBroken = false;

        foreach (var belt in _conveyorBelts)
        {
            if (belt != null) belt.StartBelt();
        }
        
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null)
            {
                buttonTask.ResetTask();
                if (buttonTask.TryGetComponent<Animator>(out Animator anim)) anim.SetBool("IsBroken", false);
            }
        }
    }

    private void HandleButtonPressed()
    {
        if (_isConveyorBroken)
        {
            _isConveyorBroken = false;

            foreach (var belt in _conveyorBelts)
            {
                if(belt != null) belt.StartBelt();
            }

            foreach (var buttonTask in _restartButtonTasks)
            {
                if (buttonTask != null && buttonTask.TryGetComponent<Animator>(out Animator anim))
                {
                    anim.SetBool("IsBroken", false); 
                }
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) buttonTask.OnSucceedAction -= HandleButtonPressed;
        }
    }
}