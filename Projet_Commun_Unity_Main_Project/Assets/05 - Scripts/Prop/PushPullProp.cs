using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PushPullProp : Prop
{
    [SerializeField] private float _moveForce;
    private readonly List<Controller> _playerControllers = new();
    
    private void MoveProp(Vector3 direction) => _rb.AddForce(direction*_moveForce, ForceMode.Force);
    
    public override void Grabbed(Controller controller)
    {
        controller.transform.parent = transform;
        
        var playerRb = controller.Rb;
        playerRb.linearVelocity = Vector3.zero;
        playerRb.isKinematic = true;
        
        Physics.IgnoreCollision(controller.Collider, _collider, true);

        PlayerMovement playerMovement = controller.PlayerMovement;
        playerMovement.IsPushPull = true;
        playerMovement.OnMove += MoveProp;
        
        _playerControllers.Add(controller);
        
        IsGrabbed = true;
    }
    
    public override void Dropped(Vector3 throwForce = default, Controller controller = null)
    {
        if(!_playerControllers.Contains(controller)) return;
        
        controller.Reset();
        controller.transform.parent = null;
        controller.Rb.isKinematic = false;
        
        Physics.IgnoreCollision(controller.Collider, _collider, false);
        
        PlayerMovement playerMovement = controller.PlayerMovement;
        playerMovement.IsPushPull = false;
        playerMovement.OnMove -= MoveProp;
        
        IsGrabbed = false;
        Debug.Log("push pull prop dropped");
    }
}