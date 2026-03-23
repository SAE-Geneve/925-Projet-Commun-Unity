using System.Collections.Generic;
using UnityEngine;

public class ConveyorBreakdownEvent : GameEvent
{
    [Header("Conveyor Settings")]
    [SerializeField] private List<GameTask> _restartButtonTasks; 
    [SerializeField] private List<ConveyorBelt> _conveyorBelts;
    [SerializeField] private List<ParticleSystem> _particlesSystems;

    private bool _isConveyorBroken = false;

    private void Start()
    {
        foreach (var p in _particlesSystems)
        {
            if(p) p.Stop();
        }
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) buttonTask.OnSucceedWithPlayer += HandleButtonPressed;
        }
    }

    public override bool IsEventActive() => _isConveyorBroken;

    public override void TriggerEvent()
    {
        Debug.Log("EVENT: Le tapis roulant tombe en panne !");
        _isConveyorBroken = true;

        foreach (var p in _particlesSystems)
        { 
            p.Play();
        }
        foreach (var belt in _conveyorBelts)
        {
            if (belt != null) belt.StopBelt();
        }
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask == null) continue;

            buttonTask.ResetTask();
            if (buttonTask.TryGetComponent<HoldInteractableTask>(out var holdTask))
                holdTask.Activate();

            if (buttonTask.TryGetComponent<Animator>(out Animator anim))
                anim.SetBool("IsBroken", true); 
        }
    }

    public override void ResetEvent()
    {
        _isConveyorBroken = false;

        foreach (var p in _particlesSystems)
        { 
            p.Stop();
        }
        foreach (var belt in _conveyorBelts)
        {
            if (belt != null) belt.StartBelt();
        }
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask == null) continue;

            buttonTask.ResetTask();
            if (buttonTask.TryGetComponent<HoldInteractableTask>(out var holdTask))
                holdTask.Deactivate();

            if (buttonTask.TryGetComponent<Animator>(out Animator anim))
                anim.SetBool("IsBroken", false);
        }
    }

    private void HandleButtonPressed(PlayerController player)
    {
        if (!_isConveyorBroken) return;

        _isConveyorBroken = false;

        foreach (var p in _particlesSystems)
        { 
            p.Stop();
        }
        foreach (var belt in _conveyorBelts)
        {
            if (belt != null) belt.StartBelt();
        }
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask == null) continue;
            if (buttonTask.TryGetComponent<HoldInteractableTask>(out var holdTask))
                holdTask.Deactivate();

            if (buttonTask.TryGetComponent<Animator>(out Animator anim))
                anim.SetBool("IsBroken", false); 
        }

        RewardPlayer(player);
    }

    private void OnDestroy()
    {
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) buttonTask.OnSucceedWithPlayer -= HandleButtonPressed;
        }
    }
}