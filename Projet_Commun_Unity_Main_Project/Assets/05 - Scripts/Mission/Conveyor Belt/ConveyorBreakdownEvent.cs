using System.Collections.Generic;
using UnityEngine;

public class ConveyorBreakdownEvent : GameEvent
{
    [Header("Conveyor Settings")]
    [SerializeField] private List<GameTask> _restartButtonTasks; 
    [SerializeField] private List<ConveyorBelt> _conveyorBelts;
    [SerializeField] private ParticleSystem _particleSystem;

    private bool _isConveyorBroken = false;

    private void Start()
    {
        if(_particleSystem) _particleSystem.Stop();
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
        _particleSystem.Play();

        foreach (var belt in _conveyorBelts) { if(belt != null) belt.StopBelt(); }

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
        _particleSystem.Stop();
        foreach (var belt in _conveyorBelts) { if (belt != null) belt.StartBelt(); }
        
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null)
            {
                buttonTask.ResetTask();
                if (buttonTask.TryGetComponent<Animator>(out Animator anim)) anim.SetBool("IsBroken", false);
            }
        }
    }
    private void HandleButtonPressed(PlayerController player)
    {
        if (_isConveyorBroken)
        {
            _isConveyorBroken = false;
            _particleSystem.Stop();
            foreach (var belt in _conveyorBelts) { if(belt != null) belt.StartBelt(); }

            foreach (var buttonTask in _restartButtonTasks)
            {
                if (buttonTask != null && buttonTask.TryGetComponent<Animator>(out Animator anim))
                    anim.SetBool("IsBroken", false); 
            }

            RewardPlayer(player);
        }
    }

    private void OnDestroy()
    {
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) buttonTask.OnSucceedWithPlayer -= HandleButtonPressed;
        }
    }
}