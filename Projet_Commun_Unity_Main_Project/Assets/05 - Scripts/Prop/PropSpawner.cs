using System.Collections;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Prop[] _propsToSpawn;
    
    [Tooltip("Does the props belongs to a PropManager")]
    [SerializeField] private PropManager _propManager;
    
    [Header("Parameters")]
    [SerializeField] [Min(0f)] private float _spawnDelay = 2f;

    public void SpawnProp()
    {
        if(_propManager && _propManager.Limited) return;
        Prop prop = Instantiate(_propsToSpawn[Random.Range(0, _propsToSpawn.Length)], transform.position, Quaternion.identity);
        if(_propManager) _propManager.AddProp(prop);
    }

    public void StartSpawnRoutine()
    {
        StartCoroutine(SpawnRoutine());
    }
    
    public void StopSpawnRoutine() => StopAllCoroutines();

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_spawnDelay);
            SpawnProp();
        }
    }
}
