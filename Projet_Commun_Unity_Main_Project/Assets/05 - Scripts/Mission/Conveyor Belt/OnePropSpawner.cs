using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class OnePropSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ConveyorProp[] _propsToSpawn;
    
    [Tooltip("Does the props belongs to a PropManager?")]
    [SerializeField] private PropManager _propManager;
    
    [Header("Parameters")]
    // [SerializeField] private bool _spawnOnEnable = true;
    // [SerializeField] private bool _useRandomDelay;
    [SerializeField] [Min(0.1f)] private float _spawnDelay = 2f;
    // [SerializeField] [Min(0.1f)] private float _minSpawnDelay = 2f;
    // [SerializeField] [Min(0.2f)] private float _maxSpawnDelay = 5f;
    
    public event Action OnPropSpawned;
    
    private ConveyorProp _conveyorProp;
    private Coroutine _spawnCoroutine;
    private bool _isSpawning;

    private void OnEnable()
    {
        if(_conveyorProp == null) SpawnLuggage();
    }

    private void SpawnLuggage()
    {
        // if(!_isSpawning || _conveyorProp || _propsToSpawn == null || _propsToSpawn.Length == 0) return;
        
        _conveyorProp = Instantiate(_propsToSpawn[Random.Range(0, _propsToSpawn.Length)], transform.position,
            transform.rotation);
        
        OnPropSpawned?.Invoke();
        
        if(_propManager) _propManager.AddProp(_conveyorProp);
    }

    private void OnTriggerExit(Collider other)
    {
        // if (!_isSpawning) return;
        
        if (_conveyorProp && other.gameObject == _conveyorProp.gameObject)
        {
            _conveyorProp = null;

            StartCoroutine(SpawnCoroutine());
        }
    }

    // public void StartSpawning()
    // {
    //     if (_isSpawning) return;
    //     _isSpawning = true;
    //
    //     if (!_conveyorProp) StartSpawnRoutine();
    // }
    //
    // public void StopSpawning()
    // {
    //     if(_isSpawning) _isSpawning = false;
    //
    //     if (_spawnCoroutine != null)
    //     {
    //         StopCoroutine(_spawnCoroutine);
    //         _spawnCoroutine = null;
    //     }
    // }
    
    private void StartSpawnRoutine()
    {
        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);

        _spawnCoroutine = StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(_spawnDelay);
        SpawnLuggage();
    }
}