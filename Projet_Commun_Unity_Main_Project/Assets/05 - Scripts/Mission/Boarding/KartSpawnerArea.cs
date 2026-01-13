using UnityEngine;

public class KartSpawnerArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PropSpawnerManager propSpawnerManager))
            propSpawnerManager.StartSpawning();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PropSpawnerManager propSpawnerManager))
            propSpawnerManager.StopSpawning();
    }
}