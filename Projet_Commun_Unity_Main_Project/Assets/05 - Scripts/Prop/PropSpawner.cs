using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Prop[] _propsToSpawn;
    
    [Tooltip("Does the props belongs to a PropManager")]
    [SerializeField] private PropManager _propManager;

    [Header("Parameters")]
    [SerializeField] private float _spawnDelay = 2f;

    private void Start() => InvokeRepeating(nameof(SpawnProp), _spawnDelay, _spawnDelay);

    private void SpawnProp()
    {
        Prop prop = Instantiate(_propsToSpawn[Random.Range(0, _propsToSpawn.Length)], transform.position, Quaternion.identity);
        if(_propManager) _propManager.AddProp(prop);
    }
}
