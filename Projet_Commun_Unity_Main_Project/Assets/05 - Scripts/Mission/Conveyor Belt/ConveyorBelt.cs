using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private Vector3 _direction;
    [SerializeField] private float _speed;
    
    private List<Prop> _props = new();

    private void FixedUpdate()
    {
        foreach (Prop prop in _props)
        {
            
        }
    }
}
