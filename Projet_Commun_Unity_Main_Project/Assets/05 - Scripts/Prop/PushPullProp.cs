using UnityEngine;

public class PushPullProp : Prop
{
    [SerializeField] private float _moveForce;
    CharacterMovement _characterMovement;
    
    private void MoveProp(Vector3 direction)
    {
        //Vector3 directionVector = new Vector3(direction.x, 0, direction.y);
        _rb.AddForce(direction*_moveForce, ForceMode.Force);
        Debug.Log(direction);
    }
    
    public override void Grabbed(PlayerController playerController)
    {
        _characterMovement = playerController.GetComponent<CharacterMovement>();
        _characterMovement.OnMove += MoveProp;
        Rigidbody playerRb = playerController.Rb;
        IsGrabbed = true;
        PlayerController = playerController;
        playerController.transform.parent = transform;
        playerRb.linearVelocity = Vector3.zero;
        playerRb.isKinematic = true;
        Debug.Log("push pull prop grabbed");
        
    }
    
    public override void Dropped(Vector3 throwForce = default)
    {
        IsGrabbed = false;
        
        if (_characterMovement)
        {
            _characterMovement.OnMove -= MoveProp;
            _characterMovement = null;
        }
        if(PlayerController)
        {
            PlayerController.Reset();
            PlayerController.Rb.isKinematic = false;
            PlayerController = null;
        }
        Debug.Log("push pull prop dropped");
    }
}
