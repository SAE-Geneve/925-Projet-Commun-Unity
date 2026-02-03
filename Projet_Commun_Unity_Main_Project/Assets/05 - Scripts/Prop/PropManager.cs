using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("The maximum props that can be spawned (set -1 for no limit)")]
    [SerializeField] [Min(-1)] int limit = -1;
    
    public bool Limited => limit > -1 && _props.Count >= limit;
    
    private readonly List<Prop> _props = new();

    public void AddProp(Prop prop)
    {
        prop.OnDestroyed += RemoveProp;
        _props.Add(prop);
        Debug.Log("Prop added: " + prop.name);
    }

    private void RemoveProp(Prop prop)
    {
        prop.OnDestroyed -= RemoveProp;
        _props.Remove(prop);
    }

    public void Clear()
    {
        for (int i = _props.Count - 1; i >= 0; i--)
        {
            var prop = _props[i];
            
            if (prop == null) continue;
            
            prop.OnDestroyed -= RemoveProp;
            
            prop.Destroy();
        }

        _props.Clear();
    }
}