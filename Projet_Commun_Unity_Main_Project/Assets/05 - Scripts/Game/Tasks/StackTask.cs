using UnityEngine;

public class StackTask : GameTask
{
    [Header("Stack Parameters")]
    [Tooltip("The prop type that needs to touch the task collider to count")]
    [SerializeField] protected PropType _propType = PropType.None;
    [Tooltip("The number of props that need to be there to succeed")]
    [SerializeField] protected int _propLimit = 20;
    private int _propCount = 0;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(Done || !other.TryGetComponent(out Prop prop) || prop.Type != _propType) return;
        
        _propCount++;
        Debug.Log($"Trigger Enter Prop {_propCount}");

        if (_propCount >= _propLimit)
        {
            Succeed();
        }
    }
    
    protected virtual void OnTriggerExit(Collider other)
    {
        if(Done || !other.TryGetComponent(out Prop prop) || prop.Type != _propType) return;
        _propCount--;
        Debug.Log($"Trigger Exit Prop {_propCount}");
    }
    
    public void SetPropType(PropType propType) => _propType = propType;
}