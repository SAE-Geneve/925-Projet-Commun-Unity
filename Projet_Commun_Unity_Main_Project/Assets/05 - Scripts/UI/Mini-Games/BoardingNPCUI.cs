using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardingNPCUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] public GameObject triggerHud; 
    [SerializeField] public Image passportSlot;
    [SerializeField] public List<Sprite> passportPhotos;

    [Header("Feedbacks")]
    [SerializeField] Image happyImage;
    [SerializeField] Image unhappyImage;
    [SerializeField] Image criminalImage;
    [SerializeField] Image arrestImage;
    [SerializeField] float effectDuration = 3f;

    private RandomCharacterSkin _skin;
    private UIFeedback _uiFeedback;
    private AudioManager _audioManager;

    void Awake()
    {
        _skin = GetComponentInParent<RandomCharacterSkin>();
        _audioManager = AudioManager.Instance;
        if (TryGetComponent(out _uiFeedback)) _uiFeedback.ImagePoolCreation(happyImage);
        
        if(triggerHud) triggerHud.SetActive(false);
    }
    
    public void ShowPassport(bool isEvil)
    {
        if(triggerHud) triggerHud.SetActive(true);
        
        if (_skin == null || passportPhotos.Count == 0) return;
        int realIndex = _skin.randomIndex;

        if (isEvil && passportPhotos.Count > 1)
        {
            int fakeIndex = realIndex;
            while (fakeIndex == realIndex) fakeIndex = Random.Range(0, passportPhotos.Count);
            passportSlot.sprite = passportPhotos[fakeIndex];
        }
        else if (realIndex < passportPhotos.Count)
        {
            passportSlot.sprite = passportPhotos[realIndex];
        }
    }
    private void HidePassport()
    {
        if(triggerHud) triggerHud.SetActive(false);
    }
    public void PlayFeedback(bool success, bool isEvil, bool accepted)
    {
        HidePassport();
        
        _audioManager?.PlaySfx(success ? _audioManager.successSFX : _audioManager.failureSFX);
        
        Image targetImage = accepted 
            ? (isEvil ? criminalImage : happyImage) 
            : (isEvil ? arrestImage : unhappyImage);

        StartCoroutine(_uiFeedback.ImageFade(targetImage, effectDuration));
    }
}