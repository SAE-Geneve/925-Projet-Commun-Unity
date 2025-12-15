using UnityEngine;

public class ConveyorBeltTask : TriggerTask
{
    private Controller _controller;
    protected override void Start()
    {
        base.Start();
        
        _controller = GetComponent<Controller>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(Done || !other.TryGetComponent(out Prop prop) || prop.Type != _propType) return;
        
        if(prop.IsGrabbed) return;
        
        Succeed();
        
        prop.Grabbed(_controller);
        _controller.SetGrabbedProp(prop);
    }
}
