using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
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
            
            prop.OnDestroyed -= RemoveProp;
            
            prop.Destroy();
        }

        _props.Clear();
    }
}
