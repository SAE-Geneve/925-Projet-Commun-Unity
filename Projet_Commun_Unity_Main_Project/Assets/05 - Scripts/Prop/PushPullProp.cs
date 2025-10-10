using UnityEngine;

public class PushPullProp : Prop
{
    private Transform player;

    void Start()
    {
        _rb.isKinematic = true;
    }
    public override void Grabbed(Catcher catcher)
    {
        IsGrabbed = true;
        _catcher = catcher;
        
        if (_rb != null) _rb.isKinematic = false;
        Debug.Log("push pull prop grabbed");
    }

    public override void Dropped(Vector3 throwForce = default)
    {
        if(_rb != null) _rb.isKinematic = true;
        IsGrabbed = false;
        if(_catcher)
        {
            _catcher.Reset();
            _catcher = null;
        }
        Debug.Log("push pull prop dropped");
    }
}
