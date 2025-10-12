using UnityEngine;

public class TriggerTask : GameTask
{
    [Header("Trigger Parameters")]
    [Tooltip("The prop type that needs to touch the task collider to succeed")]
    [SerializeField] private PropType _propType = PropType.None;

    private void OnTriggerEnter(Collider other)
    {
        if(Done || !other.TryGetComponent(out Prop prop) || prop.Type != _propType) return;

        if (prop.IsGrabbed)
            prop.Dropped();
        
        Succeed();
        Destroy(other.gameObject);
    }
}