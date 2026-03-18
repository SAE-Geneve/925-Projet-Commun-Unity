using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChariotRespawn : MonoBehaviour, IRespawnable
{
    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private Rigidbody _rb;
    void Awake()
    {
        _startPosition = _rb.position;
        _startRotation = _rb.rotation;

        _rb = GetComponent<Rigidbody>();
    }

    public void Respawn()
    {
        _rb.position = _startPosition;
        _rb.rotation = _startRotation;
        
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}
