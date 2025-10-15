using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PushPullProp : Prop
{
    [SerializeField] private float _moveForce;
    private List<PlayerController> _playerControllers = new();
    
    private void MoveProp(Vector3 direction) => _rb.AddForce(direction*_moveForce, ForceMode.Force);
    
    public override void Grabbed(PlayerController playerController)
    {
        //PlayerControllers = playerController;
        playerController.transform.parent = transform;
        _playerControllers.Add(playerController);
        IsGrabbed = true;
        var playerRb = playerController.Rb;
        playerRb.linearVelocity = Vector3.zero;
        playerRb.isKinematic = true;

        playerController.PlayerMovement.OnMove += MoveProp;
    }
    
    public override void Dropped(Vector3 throwForce = default, PlayerController playerController = null)
    {
        IsGrabbed = false;

        if(_playerControllers.Contains(playerController))
        {
            playerController.Reset();
            playerController.Rb.isKinematic = false;
            playerController.transform.parent = null;
            playerController.PlayerMovement.OnMove -= MoveProp;
            _playerControllers.Remove(playerController);
        }
        Debug.Log("push pull prop dropped");
    }
}