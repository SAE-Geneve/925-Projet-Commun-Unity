using UnityEngine;

public class PushPullProp : Prop
{
    private Transform player;

    void Start()
    {
        _rb.isKinematic = true;
    }
    public override void Grabbed(PlayerController playerController)
    {
        IsGrabbed = true;
        PlayerController = playerController;
        
        if (_rb != null) _rb.isKinematic = false;
        Debug.Log("push pull prop grabbed");
    }

    public override void Dropped(Vector3 throwForce = default)
    {
        if(_rb != null) _rb.isKinematic = true;
        IsGrabbed = false;
        if(PlayerController)
        {
            PlayerController.Reset();
            PlayerController = null;
        }
        Debug.Log("push pull prop dropped");
    }
}
