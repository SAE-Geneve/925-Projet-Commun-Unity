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
    
    private bool isActive = false;
    public bool IsActive => isActive; 
    
    public void ChangeState()
    {
        if (!canInteract) return; 

        isActive = !isActive;
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