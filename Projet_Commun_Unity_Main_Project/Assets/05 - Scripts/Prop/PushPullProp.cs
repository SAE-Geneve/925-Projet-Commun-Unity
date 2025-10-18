using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PushPullProp : Prop
{
    [SerializeField] private float _moveForce;
    private readonly List<PlayerController> _playerControllers = new();
    
    private void MoveProp(Vector3 direction) => _rb.AddForce(direction*_moveForce, ForceMode.Force);
    
    public override void Grabbed(PlayerController playerController)
    {
        playerController.transform.parent = transform;
        
        var playerRb = playerController.Rb;
        playerRb.linearVelocity = Vector3.zero;
        playerRb.isKinematic = true;
        
        Physics.IgnoreCollision(playerController.Collider, _collider, true);

        PlayerMovement playerMovement = playerController.PlayerMovement;
        playerMovement.IsPushPull = true;
        playerMovement.OnMove += MoveProp;
        
        _playerControllers.Add(playerController);
        
        IsGrabbed = true;
    }
    
    public override void Dropped(Vector3 throwForce = default, PlayerController playerController = null)
    {
        if(!_playerControllers.Contains(playerController)) return;
        
        playerController.Reset();
        playerController.transform.parent = null;
        playerController.Rb.isKinematic = false;
        
        Physics.IgnoreCollision(playerController.Collider, _collider, false);
        
        PlayerMovement playerMovement = playerController.PlayerMovement;
        playerMovement.IsPushPull = false;
        playerMovement.OnMove -= MoveProp;
        
        IsGrabbed = false;
        Debug.Log("push pull prop dropped");
    }
}