using System.Collections.Generic;
using UnityEngine;
public class KartBreakdownEvent : GameEvent
{
    [Header("Kart Settings")]
    [SerializeField] private List<GameTask> _restartButtonTasks; 
    [SerializeField] private KartMovement _kart;
    [SerializeField] private ParticleSystem _particleSystem;

    private bool _isKartBroken = false;

    private void Start()
    {
        _particleSystem.Stop();
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) buttonTask.OnSucceedWithPlayer += HandleButtonPressed;
        }
    }

    public override bool IsEventActive() => _isKartBroken;

    public override void TriggerEvent()
    {
        Debug.Log("EVENT: Le kart tombe en panne !");
        _isKartBroken = true;
        _particleSystem.Play();
        _kart.Breakdown();

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
        _isKartBroken = false;
        _particleSystem.Stop();
        _kart.Restart();
        
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
        if (_isKartBroken)
        {
            _isKartBroken = false;
            _particleSystem.Stop();
            _kart.Restart();

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
