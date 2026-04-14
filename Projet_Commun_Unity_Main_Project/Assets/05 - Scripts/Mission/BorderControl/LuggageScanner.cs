using UnityEngine;
using UnityEngine.Events;

public class LuggageScanner : MonoBehaviour
{
    [Header("Feedbacks")]
    [SerializeField] private Animator alarmAnimator;
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip beepSound;

    [Header("Events")] 
    [SerializeField] private UnityEvent onGoodProp;
    [SerializeField] private UnityEvent onBadProp;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Prop") || !other.TryGetComponent(out Prop prop)) return;
        
        PlayScanFeedbacks();
        
        if (prop.Type == PropType.GoodProp) onGoodProp.Invoke();
        else if(prop.Type == PropType.BadProp) onBadProp.Invoke();
    }

    private void PlayScanFeedbacks()
    {
        if (alarmAnimator) alarmAnimator.SetTrigger("Alert");
        if (audioSource && beepSound) audioSource.PlayOneShot(beepSound);
    }
}