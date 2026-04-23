using System;
using System.Collections.Generic;
using UnityEngine;

public class MopProp : InteractableProp
{
    [Header("Mop References")]
    [SerializeField] private Material _interactMaterial;
    [SerializeField] private ParticleSystem _particleSystem;

    [Header("Mop Parameters")]
    [SerializeField] [Min(0.1f)] private float cleanTime = 3f;

    [SerializeField] private Animator _animator;

    public event Action OnStartClean;
    public event Action OnStopClean;
    
    public float CleanTime => cleanTime;
    
    public PlayerController CurrentCleaner { get; private set; }
    
    private Renderer _renderer;
    private Material _originalMaterial;
    private List<PuddleTask> _puddleTasks = new();
    private bool _isCleaning;

    protected override void Start()
    {
        base.Start();
        _renderer = GetComponent<Renderer>();
        _originalMaterial = _renderer.material;
    }

    public override void Interact(PlayerController playerController)
    {
        if(playerController.InteractableGrabbed == null) return;
        
        CurrentCleaner = playerController;
        OnStartClean?.Invoke();
        _animator.SetBool("Swip", true);
        _isCleaning = true;
        _particleSystem.Play();
        _renderer.material = _interactMaterial;
    }

    public override void InteractEnd()
    {
        OnStopClean?.Invoke();
        CurrentCleaner = null;
        _animator.SetBool("Swip", false);
        _isCleaning = false;
        _particleSystem.Stop();
        _renderer.material = _originalMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("MopZone")) return;
    
        PuddleTask puddleTask = other.GetComponentInParent<PuddleTask>();
        if (puddleTask != null && !_puddleTasks.Contains(puddleTask))
        {
            puddleTask.Register(this);
            if(_isCleaning) puddleTask.StartClean();
            _puddleTasks.Add(puddleTask);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("MopZone")) return;
    
        PuddleTask puddleTask = other.GetComponentInParent<PuddleTask>();
        if (puddleTask != null && _puddleTasks.Contains(puddleTask))
            RemovePuddleTask(puddleTask);
    }

    public void RemovePuddleTask(PuddleTask puddleTask)
    {
        puddleTask.Unregister(this);
        if(_isCleaning) puddleTask.StopClean();
        _puddleTasks.Remove(puddleTask);
    }

    public override void Grabbed(Controller controller)
    {
        base.Grabbed(controller);
        if (controller is PlayerController pc)
        {
            pc.Ragdoll.IsImmune = true;
        }
    }

    public override void Dropped(Vector3 throwForce = default, Controller controller = null)
    {
        base.Dropped(throwForce, controller);
        if (controller is PlayerController pc)
        {
            pc.Ragdoll.IsImmune = false;
        }
    }
}