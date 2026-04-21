using UnityEngine;

public class LuggageScanner : MonoBehaviour
{
    [Header("Feedbacks")]
    [SerializeField] private Animator alarmAnimator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip beepGoodSound;
    [SerializeField] private AudioClip beepBadSound;

    [Header("Screen Connection")]
    [SerializeField] private BorderControlScreen borderScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Prop") || !other.TryGetComponent(out Prop prop)) return;

        bool isDangerous = prop.Type == PropType.BadProp;

        PlayScanFeedbacks(isDangerous);

        if (borderScreen) borderScreen.DisplayScreenForProp(prop);
    }

    private void PlayScanFeedbacks(bool isDangerous)
    {
        if (alarmAnimator)
            alarmAnimator.SetTrigger(isDangerous ? "AlertBad" : "AlertGood");

        AudioClip clip = isDangerous ? beepBadSound : beepGoodSound;
        if (audioSource && clip)
            audioSource.PlayOneShot(clip);
    }
}