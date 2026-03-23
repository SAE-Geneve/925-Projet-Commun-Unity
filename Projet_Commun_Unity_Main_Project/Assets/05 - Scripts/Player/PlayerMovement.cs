using System.Collections;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    [Header("Parameters")]
    [SerializeField] private float dashForce = 5f;
    [SerializeField] private float dashDuration = 5f;
    
    [Header("Effects")]
    [SerializeField] private ParticleSystem diveParticles;
    [SerializeField] private AudioClip dashSound;
    
    private PlayerController _player;
    private AudioSource _audioSource;
    
    private bool _isDashing;

    protected override void Start()
    {
        base.Start();
        
        _player = GetComponent<PlayerController>();
        
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        if (diveParticles != null) 
        {
            var emission = diveParticles.emission;
            emission.enabled = false;
        }
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
        
        if (dashSound != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(dashSound);
        }

        if (diveParticles != null)
        {
            var emission = diveParticles.emission;
            emission.enabled = true;
            diveParticles.Play();
        }
        
        StartCoroutine(nameof(DashCoroutine));
    
        Rb.AddForce(transform.forward * (dashForce + _player.PlayerBonus.Dive), ForceMode.Impulse);
    }
    
    private IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;
        if (diveParticles != null)
        {
            var emission = diveParticles.emission;
            emission.enabled = false;
        }
    }

    protected override Vector3 CalculateTargetVelocity(Vector3 direction)
    {
        return direction * (speed + _player.PlayerBonus.Speed);
    }
}
