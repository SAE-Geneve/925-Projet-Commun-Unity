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
    [SerializeField] [Min(0.1f)] private float _spawnDelay = 2f;
    
    public event Action OnPropSpawned;
    
    private ConveyorProp _conveyorProp;
    private Coroutine _spawnCoroutine;
    private bool _isSpawning;
    private PropType? _targetedType = null;

    private void OnEnable()
    {
        if(_conveyorProp == null) SpawnLuggage();
    }

    public void SetNextTargetedType(PropType? type)
    {
        _targetedType = type;
    }

    private void SpawnLuggage()
    {
        ConveyorProp toSpawn = null;

        if (_targetedType.HasValue)
        {
            string typeName = _targetedType.Value.ToString();

            foreach (var prefab in _propsToSpawn)
            {
                if (prefab.name.Contains(typeName))
                {
                    toSpawn = prefab;
                    break;
                }
            }

            _targetedType = null;
        }
        
        toSpawn ??= _propsToSpawn[Random.Range(0, _propsToSpawn.Length)];

        _conveyorProp = Instantiate(toSpawn, transform.position, transform.rotation);
        OnPropSpawned?.Invoke();
        if(_propManager) _propManager.AddProp(_conveyorProp);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_conveyorProp && other.gameObject == _conveyorProp.gameObject)
        {
            _conveyorProp = null;
            StartCoroutine(SpawnCoroutine());
        }
    }

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