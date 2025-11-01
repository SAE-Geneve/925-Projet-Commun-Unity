using System.Collections.Generic;
using UnityEngine;

public class PropsLimit : MonoBehaviour
{

    [SerializeField] private int _luggageCounter = 0;
    [SerializeField] private int luggageLimit = 20;
    
    private OnePropSpawner[] _spawners;
    
    private bool _done = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _spawners = GetComponentsInChildren<OnePropSpawner>();
        
        foreach (var propSpawner in _spawners)
        {
            propSpawner.OnPropSpawned += PropSpawned;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_luggageCounter >= luggageLimit)
        {
            foreach (var propSpawner in _spawners)
            {
                propSpawner.gameObject.SetActive(false);
            }
            
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
