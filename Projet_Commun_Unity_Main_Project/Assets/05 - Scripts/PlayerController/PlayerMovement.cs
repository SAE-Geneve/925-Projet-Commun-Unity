using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    [SerializeField] private float dashForce = 5f;
    [SerializeField] private float dashDuration = 5f;
    private Animator _animator;

    private bool _isDashing;

    private void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }
    
    protected override void HorizontalMovement()
    {
        if (!_isDashing)
        {
            base.HorizontalMovement();
            
            _animator.SetFloat("Speed", Mathf.Abs(Rb.linearVelocity.magnitude));
        }
    }

    public void Dash()
    {
        if (_isDashing)
        {
            return;
        }
        
        _isDashing = true;
        StartCoroutine("DashCoroutine");
        
        Rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);

    }

    private IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;
    }
}