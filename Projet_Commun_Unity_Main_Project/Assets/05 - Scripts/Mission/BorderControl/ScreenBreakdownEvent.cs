using System.Collections.Generic;
using UnityEngine;

public class ScreenBreakdownEvent : GameEvent
{
    [Header("Links")]
    [SerializeField] private AIManagerBorder _aiManagerBorder;

    [Header("Screen Event Settings")]
    [SerializeField] private GameObject _scrambledScreenVisual;
    [SerializeField] private GameObject _warningLogoVisual;
    [SerializeField] private GameTask _screenRepairTask;

    [Header("Conveyor Settings")]
    [SerializeField] private List<GameTask> _restartButtonTasks; 
    [SerializeField] private List<ConveyorBelt> _conveyorBelts;

    private bool _isScreenBroken = false;
    private bool _isConveyorStopped = false;

    private void Start()
    {
        if (_screenRepairTask != null) _screenRepairTask.OnSucceedAction += HandleScreenRepaired;
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) buttonTask.OnSucceedAction += HandleButtonPressed;
        }
    }

    public override bool IsEventActive()
    {
        return _isScreenBroken || _isConveyorStopped;
    }

    public override void TriggerEvent()
    {
        Debug.Log("EVENT: L'écran du scanner est brouillé !");
        _isScreenBroken = true;
        _isConveyorStopped = true;

        if (_aiManagerBorder != null) _aiManagerBorder.isSpawningPaused = true;

        foreach (var belt in _conveyorBelts) { if (belt != null) belt.StopBelt(); }
        
        if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(true);
        if (_warningLogoVisual != null) _warningLogoVisual.SetActive(true);

        if (_screenRepairTask != null) 
        {
            _screenRepairTask.ResetTask();
            if (_screenRepairTask.TryGetComponent<Animator>(out Animator anim)) anim.SetBool("IsBroken", true);
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

    public override void ResetEvent()
    {
        _isScreenBroken = false;
        _isConveyorStopped = false;

        if (_aiManagerBorder != null) _aiManagerBorder.isSpawningPaused = false;
        if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(false);
        if (_warningLogoVisual != null) _warningLogoVisual.SetActive(false);

        foreach (var belt in _conveyorBelts) { if (belt != null) belt.StartBelt(); }

        if (_screenRepairTask != null) 
        {
            _screenRepairTask.ResetTask();
            if (_screenRepairTask.TryGetComponent<Animator>(out Animator anim)) anim.SetBool("IsBroken", false);
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

    private void HandleScreenRepaired()
    {
        if (_isScreenBroken)
        {
            _isScreenBroken = false;
            if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(false);
            if (_warningLogoVisual != null) _warningLogoVisual.SetActive(false);

            if (_screenRepairTask != null && _screenRepairTask.TryGetComponent<Animator>(out Animator screenAnim)) screenAnim.SetBool("IsBroken", false);

            foreach (var buttonTask in _restartButtonTasks)
            {
                if (buttonTask != null && buttonTask.TryGetComponent<Animator>(out Animator beltAnim)) beltAnim.SetBool("IsBroken", true);
            }
        }
    }

    private void HandleButtonPressed()
    {
        if (_isConveyorStopped)
        {
            if (_isScreenBroken)
            {
                foreach (var buttonTask in _restartButtonTasks) { if (buttonTask != null) buttonTask.ResetTask(); }
                return; 
            }

            _isConveyorStopped = false;
            if (_aiManagerBorder != null) _aiManagerBorder.isSpawningPaused = false;

            foreach (var belt in _conveyorBelts) { if (belt != null) belt.StartBelt(); }

            foreach (var buttonTask in _restartButtonTasks)
            {
                if (buttonTask != null && buttonTask.TryGetComponent<Animator>(out Animator beltAnim)) beltAnim.SetBool("IsBroken", false);
            }
        }
    }

    private void OnDestroy()
    {
        if (_screenRepairTask != null) _screenRepairTask.OnSucceedAction -= HandleScreenRepaired;
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) buttonTask.OnSucceedAction -= HandleButtonPressed;
        }
    }
}