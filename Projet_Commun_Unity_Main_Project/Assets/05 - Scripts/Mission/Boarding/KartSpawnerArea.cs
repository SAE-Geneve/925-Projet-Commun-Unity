using UnityEngine;

public class KartSpawnerArea : MonoBehaviour
{
    private PropSpawner _propSpawner;
    
    private void Awake() => _propSpawner = GetComponentInChildren<PropSpawner>();
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KartSpawner"))
            _propSpawner.StartSpawnRoutine();
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("KartSpawner"))
            _propSpawner.StopSpawnRoutine();
    }
}