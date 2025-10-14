using UnityEngine;

public class PushPullProp : Prop
{
    [SerializeField] private float _moveForce;
    
    private void MoveProp(Vector3 direction) => _rb.AddForce(direction*_moveForce, ForceMode.Force);
    
    public override void Grabbed(PlayerController playerController)
    {
        PlayerController = playerController;
        PlayerController.transform.parent = transform;
        
        IsGrabbed = true;
        
        Rigidbody playerRb = playerController.Rb;
        playerRb.linearVelocity = Vector3.zero;
        playerRb.isKinematic = true;

        PlayerController.PlayerMovement.OnMove += MoveProp;
    }
    
    public override void Dropped(Vector3 throwForce = default)
    {
        IsGrabbed = false;

        if(PlayerController)
        {
            PlayerController.Reset();
            PlayerController.Rb.isKinematic = false;
            PlayerController.PlayerMovement.OnMove -= MoveProp;
            PlayerController = null;
        }
        Debug.Log("push pull prop dropped");
    }
}