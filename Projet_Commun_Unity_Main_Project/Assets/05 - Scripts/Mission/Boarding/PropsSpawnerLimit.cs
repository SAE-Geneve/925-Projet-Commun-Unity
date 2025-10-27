using UnityEngine;

public class PropsSpawnerLimit : MonoBehaviour
{

    private int _luggageCounter = 0;
    [SerializeField] private int luggageLimit = 20;
    
    private bool _done = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var propSpawner in GetComponentsInChildren<OnePropSpawner>())
        {
            propSpawner.OnPropSpawned += PropSpawned;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_luggageCounter >= luggageLimit)
        {
            _done = true;
        }
    }

    void PropSpawned()
    {
        _luggageCounter++;
    }

    public bool IsDone()
    {
        return _done;
    }
}
