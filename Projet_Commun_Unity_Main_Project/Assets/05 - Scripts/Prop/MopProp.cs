using System;
using System.Collections.Generic;
using UnityEngine;

public class MopProp : InteractableProp
{
    [Header("Mop References")]
    [SerializeField] private Material _interactMaterial;

    [Header("Mop Parameters")]
    [SerializeField] [Min(0.1f)] private float cleanTime = 3f;

    public event Action OnStartClean;
    public event Action OnStopClean;
    
    public float CleanTime => cleanTime;
    
    // NOUVEAU : On stocke le joueur en train de nettoyer
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
        
        CurrentCleaner = playerController; // On sauvegarde le joueur
        OnStartClean?.Invoke();
        
        _isCleaning = true;
        _renderer.material = _interactMaterial;
    }

    public override void InteractEnd()
    {
        OnStopClean?.Invoke();
        CurrentCleaner = null; // On vide le joueur
        
        _isCleaning = false;
        _renderer.material = _originalMaterial;
    }
    private void OnTriggerEnter(Collider other)
    {
        PuddleTask puddleTask = other.GetComponentInParent<PuddleTask>();
        if (puddleTask != null && other.gameObject != puddleTask.gameObject && !_puddleTasks.Contains(puddleTask))
        {
            puddleTask.Register(this);
            if(_isCleaning) puddleTask.StartClean();
            _puddleTasks.Add(puddleTask);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PuddleTask puddleTask = other.GetComponentInParent<PuddleTask>();
        if (puddleTask != null && other.CompareTag("MopZone"))
            RemovePuddleTask(puddleTask);
    }

    public void RemovePuddleTask(PuddleTask puddleTask)
    {
        puddleTask.Unregister(this);
        if(_isCleaning) puddleTask.StopClean();
        _puddleTasks.Remove(puddleTask);
    }
}