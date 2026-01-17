using UnityEngine;

public class PropSpawnerManager : MonoBehaviour
{

    // [SerializeField] private int _luggageCounter;
    // [SerializeField] private int luggageLimit = 20;
    //
    private OnePropSpawner[] _spawners;
    //
    // private bool _done;
    
    void Start()
    {
        _spawners = GetComponentsInChildren<OnePropSpawner>();
        
        // foreach (var propSpawner in _spawners)
        // {
        //     propSpawner.OnPropSpawned += PropSpawned;
        // }
    }
    
    // void Update()
    // {
    //     if (_luggageCounter >= luggageLimit)
    //     {
    //         foreach (var propSpawner in _spawners)
    //             propSpawner.gameObject.SetActive(false);
    //         
    //         _done = true;
    //     }
    // }
    //
    // void PropSpawned()
    // {
    //     _luggageCounter++;
    // }

    // public void StartSpawning()
    // {
    //     foreach (var spawner in _spawners)
    //         spawner.StartSpawning();
    // }
    //
    // public void StopSpawning()
    // {
    //     foreach (var spawner in _spawners)
    //         spawner.StopSpawning();
    // }
}