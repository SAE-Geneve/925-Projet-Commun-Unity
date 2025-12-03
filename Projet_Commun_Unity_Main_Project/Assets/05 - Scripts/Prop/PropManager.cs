using System.Collections.Generic;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    private readonly List<Prop> _props = new();

    public void AddProp(Prop prop)
    {
        prop.OnDestroyed += RemoveProp;
        _props.Add(prop);
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
            
            if(prop.IsGrabbed)
                prop.Dropped();
            
            prop.OnDestroyed -= RemoveProp;
            Destroy(prop.gameObject);
        }

        _props.Clear();
    }
}
