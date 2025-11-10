using System;
using System.Collections.Generic;
using UnityEngine;

public class MopProp : InteractableProp
{
    [Header("References")]
    [SerializeField] private Material _interactMaterial;

    public event Action OnStartClean;
    public event Action OnStopClean;
    
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
        OnStartClean?.Invoke();
        
        _isCleaning = true;
        _renderer.material = _interactMaterial;
    }

    public override void InteractEnd()
    {
        OnStopClean?.Invoke();
        
        _isCleaning = false;
        _renderer.material = _originalMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PuddleTask puddleTask))
        {
            puddleTask.Register(this);
            if(_isCleaning) puddleTask.StartClean();
            _puddleTasks.Add(puddleTask);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PuddleTask puddleTask))
            RemovePuddleTask(puddleTask);
    }

    public void RemovePuddleTask(PuddleTask puddleTask)
    {
        puddleTask.Unregister(this);
        if(_isCleaning) puddleTask.StopClean();
        _puddleTasks.Remove(puddleTask);
    }
}