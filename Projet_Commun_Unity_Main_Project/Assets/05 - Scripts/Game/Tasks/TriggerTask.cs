using UnityEngine;
using UnityEngine.Serialization;

public class TriggerTask : GameTask
{
    [Header("Trigger Parameters")]
    [Tooltip("The prop type that needs to touch the task collider to succeed")]
    [SerializeField] protected PropType _propType = PropType.None;
    
    [SerializeField] protected bool isDestroyed = true;

    private void OnTriggerEnter(Collider other)
    {
        if(Done || !other.TryGetComponent(out Prop prop) || prop.Type != _propType) return;

        if (prop.IsGrabbed)
            prop.Dropped();
        
        Succeed();
        if(isDestroyed)
            Destroy(other.gameObject);
    }
}