using System.Collections.Generic;
using UnityEngine;

public class ConveyorProp : Prop
{
    [Header("Conveyor Belt")]
    [Tooltip("The linear damping to set when the prop is on a conveyor")]
    [SerializeField] private float _conveyorLinearDamping = 10f;
    
    [Tooltip("The angular damping to set when the prop is on a conveyor")]
    [SerializeField] private float _conveyorAngularDamping = 10f;
    
    private readonly List<ConveyorBelt> _conveyorBelts = new();
    
    private float _originalLinearDamping;
    private float _originalAngularDamping;
    
    private bool _onConveyor;

    protected override void Start()
    {
        base.Start();
        _originalLinearDamping = _rb.linearDamping;
        _originalAngularDamping = _rb.angularDamping;
    }

    #region Conveyor Belt

    public void AddConveyorBelt(ConveyorBelt conveyorBelt)
    {
        _conveyorBelts.Add(conveyorBelt);

        if (!_onConveyor)
        {
            _onConveyor = true;
            _rb.linearDamping = _conveyorLinearDamping;
            _rb.angularDamping = _conveyorAngularDamping;
        }
    }

    public void RemoveConveyorBelt(ConveyorBelt conveyorBelt)
    {
        _conveyorBelts.Remove(conveyorBelt);

        if (_conveyorBelts.Count == 0)
        {
            _onConveyor = false;
            _rb.linearDamping = _originalLinearDamping;
            _rb.angularDamping = _originalAngularDamping;
        }
    }

    #endregion

    private void OnDestroy()
    {
        foreach (ConveyorBelt conveyorBelt in _conveyorBelts)
            conveyorBelt.Remove(this);
    }
}