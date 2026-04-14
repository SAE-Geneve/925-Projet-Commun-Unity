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

    [Header("Extra Score Settings")]
    [Tooltip("Score bonus spécifique pour relancer le tapis de ce niveau")]
    [SerializeField] private int _scoreRestartConveyor = 100;

    [SerializeField] private ParticleSystem _particleSystemScan;
    [SerializeField] private ParticleSystem _particleSystemConvoyor;

    private bool _isScreenBroken = false;
    private bool _isConveyorStopped = false;

    private void Start()
    {
        _particleSystemScan.Stop();
        _particleSystemConvoyor.Stop();

        if (_screenRepairTask != null) _screenRepairTask.OnSucceedWithPlayer += HandleScreenRepaired;
        foreach (var buttonTask in _restartButtonTasks)
            if (buttonTask != null) buttonTask.OnSucceedWithPlayer += HandleButtonPressed;
    }

    public override bool IsEventActive() => _isScreenBroken || _isConveyorStopped;

    public override void TriggerEvent()
    {
        Debug.Log("EVENT: L'écran du scanner est brouillé !");
        _particleSystemScan.Play();
        _particleSystemConvoyor.Play();
        _isScreenBroken = true;
        _isConveyorStopped = true;

        if (_aiManagerBorder != null) _aiManagerBorder.isSpawningPaused = true;

        foreach (var belt in _conveyorBelts) { if (belt != null) belt.StopBelt(); }

        if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(true);
        if (_warningLogoVisual != null) _warningLogoVisual.SetActive(true);

        if (_screenRepairTask != null)
        {
            _screenRepairTask.ResetTask();
            if (_screenRepairTask.TryGetComponent<HoldInteractableTask>(out var holdScreen)) holdScreen.Activate();
            if (_screenRepairTask.TryGetComponent<Animator>(out Animator anim)) anim.SetBool("IsBroken", true);
        }

        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask == null) continue;
            buttonTask.ResetTask();
            if (buttonTask.TryGetComponent<HoldInteractableTask>(out var holdTask)) holdTask.Activate();
            if (buttonTask.TryGetComponent<Animator>(out Animator anim)) anim.SetBool("IsBroken", false);
        }
    }

    public override void ResetEvent()
    {
        _particleSystemScan.Stop();
        _particleSystemConvoyor.Stop();
        _isScreenBroken = false;
        _isConveyorStopped = false;

        if (_aiManagerBorder != null) _aiManagerBorder.isSpawningPaused = false;
        if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(false);
        if (_warningLogoVisual != null) _warningLogoVisual.SetActive(false);

        foreach (var belt in _conveyorBelts) { if (belt != null) belt.StartBelt(); }

        if (_screenRepairTask != null)
        {
            _screenRepairTask.ResetTask();
            if (_screenRepairTask.TryGetComponent<HoldInteractableTask>(out var holdTask)) holdTask.Deactivate();
            if (_screenRepairTask.TryGetComponent<Animator>(out Animator anim)) anim.SetBool("IsBroken", false);
        }

        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask == null) continue;
            buttonTask.ResetTask();
            if (buttonTask.TryGetComponent<HoldInteractableTask>(out var holdTask)) holdTask.Deactivate();
            if (buttonTask.TryGetComponent<Animator>(out Animator anim)) anim.SetBool("IsBroken", false);
        }
    }

    private void HandleScreenRepaired(PlayerController player)
    {
        if (!_isScreenBroken) return;

        _particleSystemScan.Stop();
        _isScreenBroken = false;
        if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(false);
        if (_warningLogoVisual != null) _warningLogoVisual.SetActive(false);

        if (_screenRepairTask != null)
        {
            if (_screenRepairTask.TryGetComponent<HoldInteractableTask>(out var holdTask)) holdTask.Deactivate();
            if (_screenRepairTask.TryGetComponent<Animator>(out Animator screenAnim)) screenAnim.SetBool("IsBroken", false);
        }

        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask == null) continue;
            buttonTask.ResetTask();
            if (buttonTask.TryGetComponent<HoldInteractableTask>(out var holdTask)) holdTask.Activate();
            if (buttonTask.TryGetComponent<Animator>(out Animator beltAnim)) beltAnim.SetBool("IsBroken", true);
        }

        RewardPlayer(player);
    }

    private void HandleButtonPressed(PlayerController player)
    {
        if (!_isConveyorStopped) return;

        if (_isScreenBroken)
        {
            foreach (var buttonTask in _restartButtonTasks)
                if (buttonTask != null) buttonTask.ResetTask();
            return;
        }

        _isConveyorStopped = false;
        _particleSystemConvoyor.Stop();
        if (_aiManagerBorder != null) _aiManagerBorder.isSpawningPaused = false;

        foreach (var belt in _conveyorBelts) { if (belt != null) belt.StartBelt(); }

        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask == null) continue;
            if (buttonTask.TryGetComponent<HoldInteractableTask>(out var holdTask)) holdTask.Deactivate();
            if (buttonTask.TryGetComponent<Animator>(out Animator beltAnim)) beltAnim.SetBool("IsBroken", false);
        }

        RewardPlayer(player, _scoreRestartConveyor);
    }

    private void OnDestroy()
    {
        if (_screenRepairTask != null) _screenRepairTask.OnSucceedWithPlayer -= HandleScreenRepaired;
        foreach (var buttonTask in _restartButtonTasks)
            if (buttonTask != null) buttonTask.OnSucceedWithPlayer -= HandleButtonPressed;
    }
}