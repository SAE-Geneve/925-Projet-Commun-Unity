using Unity.Behavior;
using UnityEngine;

public class SecurityScanner : MonoBehaviour
{
    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("AI"))
        {
           other.TryGetComponent(out BehaviorGraphAgent agent);
           if (agent.GetVariable("IsEnemy", out BlackboardVariable<bool> variableContainer))
           {
               bool isEnemy = variableContainer.Value;
               if (isEnemy)
               {
                   Debug.Log("EnemyFound");
               }
               else
               {
                   Debug.Log("Is not enemy");
               }
           }
        }
    }
}
