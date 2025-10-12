using UnityEngine;

public class ConveyorLuggageSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ConveyorProp[] _propsToSpawn;

    [Header("Parameters")]
    [SerializeField] private float _spawnDelay = 2f;
    
    private ConveyorProp _conveyorProp;

    void Start() => SpawnLuggage();

    private void SpawnLuggage()
    {
        _conveyorProp = Instantiate(_propsToSpawn[Random.Range(0, _propsToSpawn.Length)], transform.position, Quaternion.identity);
        _conveyorProp.OnGrab += GrabbedProp;
    }

    private void GrabbedProp()
    {
        _conveyorProp.OnGrab -= GrabbedProp;
        _conveyorProp = null;

        Invoke(nameof(SpawnLuggage), _spawnDelay);
    }
}
