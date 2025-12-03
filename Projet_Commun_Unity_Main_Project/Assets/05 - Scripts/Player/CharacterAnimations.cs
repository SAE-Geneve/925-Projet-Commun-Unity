using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    private CharacterMovement _characterMovement;
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    { 
        _animator.SetFloat("Speed", Mathf.Abs(_characterMovement.Velocity.magnitude) > Mathf.Epsilon ? Mathf.Abs(_characterMovement.Velocity.magnitude) : 0);
    }
}
