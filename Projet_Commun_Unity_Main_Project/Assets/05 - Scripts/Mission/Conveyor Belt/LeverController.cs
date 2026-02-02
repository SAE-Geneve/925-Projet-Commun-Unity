using System;
using UnityEngine;

public class LevierController : MonoBehaviour
{
    [Header("Réglages")]
    public Animator animator;
    
    [Header("Connexions")]
    public AIManager aiManager; 

    [NonSerialized]public string triggerOn = "Activate";
    [NonSerialized]public string triggerOff = "Desactivate";
    
    private bool isActive = false;
    
    public bool IsActive => isActive; 
    
    public void ChangeState()
    {
        isActive = !isActive;

        if (isActive)
        {
            animator.SetTrigger(triggerOn);
            if (aiManager != null)
            {
                aiManager.StartSpawn();
            }
        }
        else
        {
            animator.SetTrigger(triggerOff);
            if (aiManager != null)
            {
                aiManager.StopSpawn();
            }
        }
    }
}