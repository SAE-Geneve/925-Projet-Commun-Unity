using System;
using UnityEngine;

public class LevierController : MonoBehaviour
{
    [Header("Réglages")]
    public Animator animator;
    
    [Tooltip("Définit si le levier peut être utilisé ou non.")]
    public bool canInteract = true; 
    
    [Header("Connexions")]
    public AIManager aiManager; 

    [NonSerialized]public string triggerOn = "Activate";
    [NonSerialized]public string triggerOff = "Desactivate";

    private AudioManager _audioManager;
    
    private bool isActive = false;
    public bool IsActive => isActive; 
    
    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }
    public void ChangeState()
    {
        if (!canInteract) return; 

        isActive = !isActive;
        if (isActive)
        {
            _audioManager.PlaySfx(_audioManager.LevierOnSFX);
        }
        else
        {
            _audioManager.PlaySfx(_audioManager.LevierOffSFX);
        }
        UpdateVisualsAndLogic();
    }

    private void UpdateVisualsAndLogic()
    {
        if (isActive)
        {
            animator.SetTrigger(triggerOn);
            if (aiManager != null) aiManager.StartSpawn();
        }
        else
        {
            animator.SetTrigger(triggerOff);
            if (aiManager != null) aiManager.StopSpawn();
        }
    }

    public void UnlockLever()
    {
        canInteract = true;
    }

    public void DeactivateAndLock()
    {
        canInteract = false;

        if (isActive)
        {
            isActive = false;
            UpdateVisualsAndLogic();
        }
    }
}