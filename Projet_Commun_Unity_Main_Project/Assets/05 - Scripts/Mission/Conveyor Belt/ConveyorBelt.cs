using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private Vector3 _direction = Vector3.forward;
    [SerializeField] private float _speed = 7f;
    
    private readonly List<Prop> _props = new();

    private void FixedUpdate()
    {
        foreach (Prop prop in _props)
            prop.Rb.AddForce(_direction.normalized * _speed, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Prop" || !other.TryGetComponent(out ConveyorProp prop)) return;
        
        prop.AddConveyorBelt(this);
        _props.Add(prop);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Prop" || !other.TryGetComponent(out ConveyorProp prop)) return;
        
        prop.RemoveConveyorBelt(this);
        Remove(prop);
    }
    
    public void Remove(ConveyorProp prop) => _props.Remove(prop);
}
