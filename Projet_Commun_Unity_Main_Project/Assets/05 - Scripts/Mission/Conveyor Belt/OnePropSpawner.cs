using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class OnePropSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ConveyorProp[] _propsToSpawn;
    
    [Tooltip("Does the props belongs to a PropManager")]
    [SerializeField] private PropManager _propManager;

    [Header("Parameters")]
    [SerializeField] private float _spawnDelay = 2f;
    
    private ConveyorProp _conveyorProp;
    
    public event Action OnPropSpawned;

    private void OnEnable()
    {
        if(_conveyorProp == null) SpawnLuggage();
    }

    private void SpawnLuggage()
    {
        _conveyorProp = Instantiate(_propsToSpawn[Random.Range(0, _propsToSpawn.Length)], transform.position,
            transform.rotation);
        
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

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(_spawnDelay);
        SpawnLuggage();
    }
}
