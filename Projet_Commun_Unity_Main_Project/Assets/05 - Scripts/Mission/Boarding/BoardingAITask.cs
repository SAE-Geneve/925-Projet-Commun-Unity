using Unity.Behavior;
using UnityEngine;

public class BoardingAITask : GameTask
{
    [Header("Boarding AI")]
    [SerializeField] private bool checkEnemies;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out AIController ai)) return;

        if (!checkEnemies) SuccessAI(ai);
        else if (ai.BehaviorAgent.GetVariable("IsEnemy", out BlackboardVariable<bool> variable) && !variable.Value) SuccessAI(ai);
    }

    private void SuccessAI(AIController ai)
    {
        Succeed();
        ai.DestroyAI();
    }
}