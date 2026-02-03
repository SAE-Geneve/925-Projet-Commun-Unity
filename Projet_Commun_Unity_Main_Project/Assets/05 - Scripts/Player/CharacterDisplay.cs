    using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{
    [Header("References")]
     [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    
    private CharacterMovement _characterMovement;
    private Animator _animator;
    
    public SkinnedMeshRenderer SkinnedMeshRenderer => skinnedMeshRenderer;

    void Start()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    { 
        _animator.SetFloat("Speed", Mathf.Abs(_characterMovement.Velocity.magnitude) > Mathf.Epsilon ? Mathf.Abs(_characterMovement.Velocity.magnitude) : 0);
    }
}
