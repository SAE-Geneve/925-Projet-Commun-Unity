using System.Collections;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    [SerializeField] private float dashForce = 5f;
    [SerializeField] private float dashDuration = 5f;
    
    private Animator _animator;
    
    private bool _isDashing;
    

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }
    
    protected override void HorizontalMovement()
    {
        
        Vector2 velocity = new Vector2(Rb.linearVelocity.x, Rb.linearVelocity.z);
        if (!_isDashing)
        {
            base.HorizontalMovement();
            if (_animator)
            {
                _animator.SetFloat("Speed", Mathf.Abs(velocity.magnitude) > Mathf.Epsilon ? Mathf.Abs(velocity.magnitude) : 0);
            }
        }
    }

    public void Dash()
    {
        if (!Rb || _isDashing) return;
    
        _isDashing = true;
        StartCoroutine(nameof(DashCoroutine));
    
        Rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);
    }

    protected override void RotateCharacter()
    {
        if(!IsPushPull) base.RotateCharacter();
    }

    private IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;
    }
}