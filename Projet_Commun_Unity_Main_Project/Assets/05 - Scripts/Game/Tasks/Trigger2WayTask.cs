using UnityEngine;

public class Trigger2WayTask : TriggerTask
{
    [Header("Trigger 2 Way Task")]
    [SerializeField] private PropType _badPropType = PropType.None;
    
    private void OnTriggerEnter(Collider other)
    {
        if(Done || !other.TryGetComponent(out Prop prop)) return;
        
        if(prop.Type == _propType) Succeed();
        else if(prop.Type == _badPropType) Failed();
        else return;
        
        if (prop.IsGrabbed)
            prop.Dropped();
        
        Destroy(other.gameObject);
    }
}
