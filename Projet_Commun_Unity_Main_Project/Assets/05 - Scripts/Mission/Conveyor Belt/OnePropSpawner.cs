using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class OnePropSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ConveyorProp[] _propsToSpawn;

    [Header("Parameters")]
    [SerializeField] private float _spawnDelay = 2f;
    
    private ConveyorProp _conveyorProp;
    
    public event Action OnPropSpawned;

    void Start() => SpawnLuggage();

    private void SpawnLuggage()
    {
        _conveyorProp = Instantiate(_propsToSpawn[Random.Range(0, _propsToSpawn.Length)], transform.position,
            transform.rotation);
        
        OnPropSpawned?.Invoke();
    }


    private void OnTriggerExit(Collider other)
    {
        if (_conveyorProp && other.gameObject == _conveyorProp.gameObject)
        {
            _conveyorProp = null;
            Invoke(nameof(SpawnLuggage), _spawnDelay);
        }
    }
}
