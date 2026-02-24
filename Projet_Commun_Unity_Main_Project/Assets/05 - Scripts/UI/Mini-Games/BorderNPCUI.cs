using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class BorderNPCUI : MonoBehaviour
{
    [Header("UI Effect Elements")]
    [SerializeField] private Image happyImage;
    [SerializeField] private Image unhappyImage;
    [SerializeField] private Image criminalImage;
    [SerializeField] private Image arrestImage;
    
    [SerializeField] private float effectDuration=3f;
    
    //Components
    private AudioManager _audioManager;
    private UIFeedback _uiFeedback;

    void Start()
    {
        if (TryGetComponent(out _uiFeedback))
        {
            Debug.Log("Found UI Text Effects");
            _uiFeedback.ImagePoolCreation(happyImage);
        }
        _audioManager = AudioManager.Instance;
    }
    
    public void PlayFeedback(bool success, bool isEvil, bool accepted)
    {
        _audioManager?.PlaySfx(success ? _audioManager.successSFX : _audioManager.failureSFX);
        
        Image targetImage = accepted 
            ? (isEvil ? criminalImage : happyImage) 
            : (isEvil ? arrestImage : unhappyImage);

        StartCoroutine(_uiFeedback.ImageFade(targetImage, effectDuration));
    }
}