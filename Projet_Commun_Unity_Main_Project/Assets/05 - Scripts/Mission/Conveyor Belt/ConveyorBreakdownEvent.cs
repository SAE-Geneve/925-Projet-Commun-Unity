using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBreakdownEvent : GameEvent
{
    [Header("Conveyor Settings")]
    [SerializeField] private List<GameTask> _restartButtonTasks; 
    [SerializeField] private List<ConveyorBelt> _conveyorBelts;
    [SerializeField] private List<ParticleSystem> _particlesSystems;

    [Header("Escalation Settings")]
    [Tooltip("Délai en secondes avant que la panne empire (phase 2)")]
    [SerializeField] private float _escalationDelay = 5f;

    [Header("Shrink Settings")]
    [Tooltip("Intervalle en secondes entre chaque tick de rapetissement (phase 2)")]
    [SerializeField] private float _shrinkInterval = 2f;
    [Tooltip("Durée de l'animation de rapetissement visuel à chaque tick")]
    [SerializeField] private float _shrinkAnimDuration = 0.5f;
    [Tooltip("Pourcentage de taille perdu visuellement à chaque tick (0.15 = perd 15%)")]
    [SerializeField] private float _shrinkFactorPerTick = 0.15f;
    [Tooltip("Nombre de ticks de panne pour équivaloir 1 passage dans un LuggageRemover")]
    [SerializeField] private int _ticksPerLap = 2;
    [Tooltip("Durée de l'animation de destruction quand la valise atteint le max de laps")]
    [SerializeField] private float _destroyAnimDuration = 0.5f;

    private bool _isConveyorBroken = false;
    private Coroutine _escalationCoroutine;
    private Coroutine _shrinkCoroutine;

    private LuggageRemover _luggageRemover;
    private int _maxCheckpoints;

    private ParticleSystem.MinMaxGradient[] _defaultColors;

    private void Start()
    {
        foreach (var p in _particlesSystems)
            if (p) p.Stop();

        _defaultColors = new ParticleSystem.MinMaxGradient[_particlesSystems.Count];
        for (int i = 0; i < _particlesSystems.Count; i++)
            if (_particlesSystems[i] != null)
                _defaultColors[i] = _particlesSystems[i].main.startColor;

        foreach (var buttonTask in _restartButtonTasks)
            if (buttonTask != null) buttonTask.OnSucceedWithPlayer += HandleButtonPressed;

        _luggageRemover = FindObjectOfType<LuggageRemover>();
        _maxCheckpoints = (_luggageRemover != null ? _luggageRemover.MaxCheckpoints : 3) - 1;
    }

    private void RestoreParticleColors()
    {
        for (int i = 0; i < _particlesSystems.Count; i++)
        {
            if (_particlesSystems[i] == null) continue;
            var main = _particlesSystems[i].main;
            main.startColor = _defaultColors[i];
        }
    }

    public override bool IsEventActive() => _isConveyorBroken;

    public override void TriggerEvent()
    {
        _isConveyorBroken = true;

        AudioManager.Instance.PlaySfx(AudioManager.Instance.CarpetBreakSFX);
        
        foreach (var p in _particlesSystems)
            if (p) p.Play();

        foreach (var belt in _conveyorBelts)
            if (belt != null) belt.StopBelt();

        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask == null) continue;
            buttonTask.ResetTask();
            if (buttonTask.TryGetComponent<HoldInteractableTask>(out var holdTask))
                holdTask.Activate();
            if (buttonTask.TryGetComponent<Animator>(out var anim))
                anim.SetBool("IsBroken", true); 
        }

        if (_escalationCoroutine != null) StopCoroutine(_escalationCoroutine);
        _escalationCoroutine = StartCoroutine(EscalationRoutine());
    }

    private IEnumerator EscalationRoutine()
    {
        yield return new WaitForSeconds(_escalationDelay);

        if (!_isConveyorBroken) yield break;

        
        AudioManager.Instance.PlaySfx(AudioManager.Instance.WorsenCarpetBreakSFX);
        
        foreach (var p in _particlesSystems)
        {
            if (!p) continue;
            var main = p.main;
            main.startColor = Color.red;
        }

        if (_shrinkCoroutine != null) StopCoroutine(_shrinkCoroutine);
        _shrinkCoroutine = StartCoroutine(ShrinkPropsRoutine());
    }

    public override void ResetEvent()
    {
        _isConveyorBroken = false;
        StopEscalation();

        foreach (var p in _particlesSystems)
            if (p) p.Stop();

        RestoreParticleColors();

        foreach (var belt in _conveyorBelts)
            if (belt != null) belt.StartBelt();

        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask == null) continue;
            buttonTask.ResetTask();
            if (buttonTask.TryGetComponent<HoldInteractableTask>(out var holdTask))
                holdTask.Deactivate();
            if (buttonTask.TryGetComponent<Animator>(out var anim))
                anim.SetBool("IsBroken", false);
        }
    }

    private void HandleButtonPressed(PlayerController player)
    {
        if (!_isConveyorBroken) return;

        _isConveyorBroken = false;
        StopEscalation();

        foreach (var p in _particlesSystems)
            if (p) p.Stop();

        AudioManager.Instance.PlaySfx(AudioManager.Instance.FinishedRepairSFX);
        RestoreParticleColors();

        foreach (var belt in _conveyorBelts)
            if (belt != null) belt.StartBelt();

        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask == null) continue;
            if (buttonTask.TryGetComponent<HoldInteractableTask>(out var holdTask))
                holdTask.Deactivate();
            if (buttonTask.TryGetComponent<Animator>(out var anim))
                anim.SetBool("IsBroken", false); 
        }

        RewardPlayer(player);
    }

    private IEnumerator ShrinkPropsRoutine()
    {
        var wait = new WaitForSeconds(_shrinkInterval);

        while (_isConveyorBroken)
        {
            yield return wait;
            if (!_isConveyorBroken) yield break;

            foreach (var belt in _conveyorBelts)
            {
                if (belt == null) continue;

                var props = belt.GetProps();
                foreach (var prop in props)
                {
                    if (prop == null) continue;
                    if (prop is not ConveyorProp conveyorProp) continue;

                    conveyorProp.BreakdownTicks++;

                    if (conveyorProp.BreakdownTicks >= _ticksPerLap)
                    {
                        conveyorProp.BreakdownTicks = 0;
                        conveyorProp.Lap++;

                        if (conveyorProp.Lap >= _maxCheckpoints)
                        {
                            conveyorProp.enabled = false;
                            StartCoroutine(AnimateAndDestroy(conveyorProp.gameObject));
                            continue;
                        }
                    }

                    StartCoroutine(AnimateShrink(conveyorProp.transform));
                }
            }
        }
    }

    private IEnumerator AnimateShrink(Transform target)
    {
        if (target == null) yield break;

        Transform visual = target.childCount > 0 ? target.GetChild(0) : target;

        Vector3 startScale = visual.localScale;
        Vector3 targetScale = startScale * (1f - _shrinkFactorPerTick);
        float timer = 0f;

        while (timer < _shrinkAnimDuration)
        {
            if (visual == null) yield break;
            timer += Time.deltaTime;
            visual.localScale = Vector3.Lerp(startScale, targetScale, timer / _shrinkAnimDuration);
            yield return null;
        }

        if (visual != null)
            visual.localScale = targetScale;
    }

    private IEnumerator AnimateAndDestroy(GameObject target)
    {
        if (target == null) yield break;

        Collider col = target.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = target.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Vector3 originalScale = target.transform.localScale;
        float timer = 0f;

        while (timer < _destroyAnimDuration)
        {
            if (target == null) yield break;
            timer += Time.deltaTime;
            target.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, timer / _destroyAnimDuration);
            yield return null;
        }

        if (target != null) Destroy(target);
    }

    private void StopEscalation()
    {
        if (_escalationCoroutine != null)
        {
            StopCoroutine(_escalationCoroutine);
            _escalationCoroutine = null;
        }
        StopShrink();
    }

    private void StopShrink()
    {
        if (_shrinkCoroutine == null) return;
        StopCoroutine(_shrinkCoroutine);
        _shrinkCoroutine = null;
    }

    private void OnDestroy()
    {
        foreach (var buttonTask in _restartButtonTasks)
            if (buttonTask != null) buttonTask.OnSucceedWithPlayer -= HandleButtonPressed;
    }
}