using System.Collections;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    [Header("Parameters")]
    [SerializeField] private float dashForce = 5f;
    [SerializeField] private float dashDuration = 5f;
    
    private PlayerController _player;
    
    private bool _isDashing;

    protected override void Start()
    {
        base.Start();
        
        _player = GetComponent<PlayerController>();
    }

    protected override void HorizontalMovement()
    {
        if(FreeMovement) return;
        
        if (!_isDashing)
        {
            base.HorizontalMovement();
        }
    }

    public void Dash()
    {
        if (!Rb || _isDashing) return;
    
        _isDashing = true;
        StartCoroutine(nameof(DashCoroutine));
    
        Rb.AddForce(transform.forward * (dashForce + _player.PlayerBonus.Dive), ForceMode.Impulse);
    }
    
    private IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;
    }

    protected override Vector3 CalculateTargetVelocity(Vector3 direction)
    {
        return direction * (speed + _player.PlayerBonus.Speed);
    }
}