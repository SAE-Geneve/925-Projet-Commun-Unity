using System;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorProp : Prop
{
    [Header("Conveyor Belt Settings")]
    [SerializeField] private float _conveyorLinearDamping = 10f;
    [SerializeField] private float _conveyorAngularDamping = 10f;

    [NonSerialized] public int Lap = 0;
    
    private readonly List<ConveyorBelt> _conveyorBelts = new();
    private float _originalLinearDamping;
    private float _originalAngularDamping;
    private bool _onConveyor;

    protected override void Start()
    {
        base.Start();
        if (_rb != null) 
        {
            _originalLinearDamping = _rb.linearDamping;
            _originalAngularDamping = _rb.angularDamping;
        }
    }

    public void AddConveyorBelt(ConveyorBelt conveyorBelt)
    {
        _conveyorBelts.Add(conveyorBelt);

        if (!_onConveyor && _rb != null)
        {
            _onConveyor = true;
            _rb.linearDamping = _conveyorLinearDamping;
            _rb.angularDamping = _conveyorAngularDamping;
        }
    }

    public void RemoveConveyorBelt(ConveyorBelt conveyorBelt)
    {
        _conveyorBelts.Remove(conveyorBelt);

        if (_conveyorBelts.Count == 0 && _rb != null)
        {
            _onConveyor = false;
            _rb.linearDamping = _originalLinearDamping;
            _rb.angularDamping = _originalAngularDamping;
        }
    }

    protected override void OnDestroy()
    {
        foreach (ConveyorBelt conveyorBelt in _conveyorBelts)
        {
            if (conveyorBelt != null) conveyorBelt.Remove(this);
        }
    }
}