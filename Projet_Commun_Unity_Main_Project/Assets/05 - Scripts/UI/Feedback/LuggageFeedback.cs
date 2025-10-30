using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LuggageFeedback : MonoBehaviour
{
    [SerializeField] private float effectDuration=2.5f;
    
    [Header("Reaction Images")]
    [SerializeField] private Image happyImage;
    [SerializeField] private Image unhappyImage;
    
    private AudioManager _audioManager;
    private UIFeedback _uiFeedback;
    
    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _uiFeedback = GetComponent<UIFeedback>();
    }
    public void HappyResult()
    {
        _audioManager.PlaySfx(_audioManager.successSFX);
        StartCoroutine(_uiFeedback.ImageUpFade(happyImage, effectDuration));
    }
    public void UnhappyResult()
    {
        _audioManager.PlaySfx(_audioManager.failureSFX);
        StartCoroutine(_uiFeedback.ImageUpFade(unhappyImage, effectDuration));
    }
}
