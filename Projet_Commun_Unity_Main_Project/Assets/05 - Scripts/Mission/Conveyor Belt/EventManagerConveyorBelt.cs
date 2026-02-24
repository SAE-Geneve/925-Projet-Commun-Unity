using System.Collections.Generic;
using UnityEngine;

public class EventManagerConveyor : EventManager
{
    [Header("Event Weights")]
    [SerializeField] private float _puddleWeight = 70f;
    [SerializeField] private float _breakdownWeight = 30f;

    [Header("Puddle Settings")]
    [SerializeField] private PuddleTask _puddlePrefab;
    [SerializeField] private List<Transform> _puddleSpawnPoints;
    [SerializeField] private int _maxConcurrentPuddles = 3;

    [Header("Conveyor Settings")]
    [SerializeField] private List<GameTask> _restartButtonTasks; 
    [SerializeField] private List<ConveyorBelt> _conveyorBelts;

    [Header("Prop Reset Settings")]
    [SerializeField] private List<Prop> _mopsToReset;

    private List<Transform> _availableSpawnPoints = new();
    private List<PuddleTask> _activePuddlesList = new();
    private bool _isConveyorBroken = false;

    protected void Start()
    {
        _availableSpawnPoints.AddRange(_puddleSpawnPoints);
        
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) buttonTask.OnSucceedAction += HandleButtonPressed;
        }
    }

    public override void ResetManager()
    {
        base.ResetManager(); 

        foreach (var puddle in _activePuddlesList)
        {
            if (puddle != null) Destroy(puddle.gameObject);
        }
        _activePuddlesList.Clear();
        
        _availableSpawnPoints.Clear();
        _availableSpawnPoints.AddRange(_puddleSpawnPoints);

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

                if (buttonTask.TryGetComponent<Animator>(out Animator anim))
                {
                    anim.SetBool("IsBroken", false);
                }
            }
        }

        foreach (var mop in _mopsToReset)
        {
            if (mop != null) mop.Respawn();
        }
    }

    protected override void TriggerRandomEvent()
    {
        if (_isConveyorBroken)
        {
            SpawnPuddle();
            return;
        }

        float totalWeight = _puddleWeight + _breakdownWeight;
        float randomValue = Random.Range(0f, totalWeight);

        if (randomValue <= _puddleWeight) SpawnPuddle();
        else BreakConveyorBelt();
    }

    #region Puddle Logic
    private void SpawnPuddle()
    {
        if (_activePuddlesList.Count >= _maxConcurrentPuddles || _availableSpawnPoints.Count == 0) return;

        int randomIndex = Random.Range(0, _availableSpawnPoints.Count);
        Transform spawnPoint = _availableSpawnPoints[randomIndex];
        _availableSpawnPoints.RemoveAt(randomIndex);

        PuddleTask newPuddle = Instantiate(_puddlePrefab, spawnPoint.position, spawnPoint.rotation);
        
        _activePuddlesList.Add(newPuddle);

        newPuddle.OnSucceedAction += () => OnPuddleCleaned(spawnPoint, newPuddle);
    }

    private void OnPuddleCleaned(Transform freedSpawnPoint, PuddleTask puddle)
    {
        _activePuddlesList.Remove(puddle);
        _availableSpawnPoints.Add(freedSpawnPoint);
    }
    #endregion

    #region Conveyor Logic
    private void BreakConveyorBelt()
    {
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
                
                if (buttonTask.TryGetComponent<Animator>(out Animator anim))
                {
                    anim.SetBool("IsBroken", true); 
                }
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
    #endregion

    private void OnDestroy()
    {
        foreach (var buttonTask in _restartButtonTasks)
        {
            if (buttonTask != null) buttonTask.OnSucceedAction -= HandleButtonPressed;
        }
    }
}