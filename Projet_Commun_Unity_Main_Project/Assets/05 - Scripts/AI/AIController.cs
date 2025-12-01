using UnityEngine;
using Unity.Behavior;

public class AIController : MonoBehaviour
{
    protected BehaviorGraphAgent _behaviorAgent;

    protected virtual void Awake()
    {
        _behaviorAgent = GetComponent<BehaviorGraphAgent>();
        if (_behaviorAgent == null)
        {
            Debug.LogError($"BehaviorGraphAgent manquant sur {gameObject.name}", this);
        }
    }
}