using UnityEngine;

public class ScreenBreakdownEvent : GameEvent
{
    [Header("Links")]
    [SerializeField] private AIManagerBorder _aiManagerBorder;

    [Header("Screen Event Settings")]
    [SerializeField] private GameObject _scrambledScreenVisual;
    [SerializeField] private GameObject _warningLogoVisual;
    [SerializeField] private GameTask _screenRepairTask;

    [SerializeField] private ParticleSystem _particleSystemScan;

    private bool _isScreenBroken = false;

    private void Start()
    {
        _particleSystemScan.Stop();
        if (_screenRepairTask != null) _screenRepairTask.OnSucceedWithPlayer += HandleScreenRepaired;
    }

    public override bool IsEventActive() => _isScreenBroken;

    public override void TriggerEvent()
    {
        _isScreenBroken = true;
        _particleSystemScan.Play();

        if (_aiManagerBorder != null) _aiManagerBorder.isSpawningPaused = true;
        if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(true);
        if (_warningLogoVisual != null) _warningLogoVisual.SetActive(true);

        if (_screenRepairTask != null)
        {
            _screenRepairTask.ResetTask();
            if (_screenRepairTask.TryGetComponent<HoldInteractableTask>(out var hold)) hold.Activate();
            if (_screenRepairTask.TryGetComponent<Animator>(out var anim)) anim.SetBool("IsBroken", true);
        }
    }

    public override void ResetEvent()
    {
        _isScreenBroken = false;
        _particleSystemScan.Stop();

        if (_aiManagerBorder != null) _aiManagerBorder.isSpawningPaused = false;
        if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(false);
        if (_warningLogoVisual != null) _warningLogoVisual.SetActive(false);

        if (_screenRepairTask != null)
        {
            _screenRepairTask.ResetTask();
            if (_screenRepairTask.TryGetComponent<HoldInteractableTask>(out var hold)) hold.Deactivate();
            if (_screenRepairTask.TryGetComponent<Animator>(out var anim)) anim.SetBool("IsBroken", false);
        }
    }

    private void HandleScreenRepaired(PlayerController player)
    {
        if (!_isScreenBroken) return;

        _isScreenBroken = false;
        _particleSystemScan.Stop();

        if (_aiManagerBorder != null) _aiManagerBorder.isSpawningPaused = false;
        if (_scrambledScreenVisual != null) _scrambledScreenVisual.SetActive(false);
        if (_warningLogoVisual != null) _warningLogoVisual.SetActive(false);

        if (_screenRepairTask != null)
        {
            if (_screenRepairTask.TryGetComponent<HoldInteractableTask>(out var hold)) hold.Deactivate();
            if (_screenRepairTask.TryGetComponent<Animator>(out var anim)) anim.SetBool("IsBroken", false);
        }

        RewardPlayer(player);
    }

    private void OnDestroy()
    {
        if (_screenRepairTask != null) _screenRepairTask.OnSucceedWithPlayer -= HandleScreenRepaired;
    }
}