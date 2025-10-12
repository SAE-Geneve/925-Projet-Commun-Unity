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
            prop.Rb.AddForce(_direction * _speed, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject obj = other.gameObject;
        if(obj.tag != "Prop" || !obj.TryGetComponent(out Prop prop)) return;
        
        _props.Add(prop);
    }

    private void OnCollisionExit(Collision other)
    {
        GameObject obj = other.gameObject;
        if(obj.tag != "Prop" || !obj.TryGetComponent(out Prop prop)) return;
        
        _props.Remove(prop);
    }
}
