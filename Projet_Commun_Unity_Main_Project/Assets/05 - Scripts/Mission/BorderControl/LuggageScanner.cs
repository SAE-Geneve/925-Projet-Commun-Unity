using UnityEngine;

public class LuggageScanner : MonoBehaviour
{
    [Header("Feedbacks")]
    [SerializeField] private Animator alarmAnimator;
    [SerializeField] private AudioSource audioSource; 
    [SerializeField] private AudioClip beepSound;

    [Header("Screen Connection")]
    [SerializeField] private BorderControlScreen borderScreen; 

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Prop") || !other.TryGetComponent(out Prop prop)) return;
        
        PlayScanFeedbacks();
        
        if (borderScreen) borderScreen.DisplayScreenForProp(prop);
    }

    private void PlayScanFeedbacks()
    {
        if (alarmAnimator) alarmAnimator.SetTrigger("Alert");
        if (audioSource && beepSound) audioSource.PlayOneShot(beepSound);
    }
}