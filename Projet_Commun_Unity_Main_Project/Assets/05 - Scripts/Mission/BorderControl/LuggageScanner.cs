using UnityEngine;
using UnityEngine.Events;

public class LuggageScanner : MonoBehaviour
{
    [Header("Events")] 
    [SerializeField] private UnityEvent onGoodProp;
    [SerializeField] private UnityEvent onBadProp;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Prop") || !other.TryGetComponent(out Prop prop)) return;
        
        if (prop.Type == PropType.GoodProp) onGoodProp.Invoke();
        else onBadProp.Invoke();
    }
}