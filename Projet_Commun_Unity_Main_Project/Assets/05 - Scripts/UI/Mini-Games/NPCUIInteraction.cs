using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCUIInteraction : MonoBehaviour
{
    [Header("NPC UI")]
    [SerializeField] GameObject triggerHud;
    
    [Header("UI Slots Elements")]
    public List<Sprite> badItems;
    public List<Sprite> goodItems;
    public List<Image> slots;
    
    [Header("UI Effect Elements")]
    [SerializeField] private Image happyImage;
    [SerializeField] private Image unhappyImage;
    [SerializeField] private Image criminalImage;
    [SerializeField] private Image arrestImage;
    
    [SerializeField] private float effectDuration=2.5f;
    
    //Bool values
    private bool _inRange = false;
    private bool _resultTakenIn = false;
    
    //Components
    private PlayerInput _playerInput;
    private AudioManager _audioManager;
    private UIFeedback _uiFeedback;
    
    //Randomness for slots setup
    private int _random;
    private int _randomEvilSlot;

    void Start()
    {
        _audioManager = AudioManager.Instance;
        _uiFeedback = transform.GetComponent<UIFeedback>();
        _playerInput = FindAnyObjectByType<PlayerInput>();
        
        EvilNpcSetup();
    }

    void Update()
    {
        if (_inRange && _playerInput.actions["Interact"].triggered)
        {
            Debug.Log("Interact");
            _resultTakenIn = true;
            triggerHud.SetActive(false);
            ArrestResult();
        }
    }

    void EvilNpcSetup()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            _random = Random.Range(0, goodItems.Count);
            slots[i].sprite = goodItems[_random];
        }
        _randomEvilSlot=Random.Range(0, slots.Count);
        _random = Random.Range(0, badItems.Count);
        slots[_randomEvilSlot].sprite = badItems[_random];
    }

    void GoodNpcSetup()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            _random = Random.Range(0, goodItems.Count);
            slots[i].sprite = goodItems[_random];
        }
    }
    
    public void HappyResult()
    {
        _audioManager.PlaySfx(_audioManager.successSFX);
        StartCoroutine(_uiFeedback.ImageFade(happyImage, effectDuration));
    }
    public void UnhappyResult()
    {
        _audioManager.PlaySfx(_audioManager.failureSFX);
        StartCoroutine(_uiFeedback.ImageFade(unhappyImage, effectDuration));
    }
    public void ArrestResult()
    {
        _audioManager.PlaySfx(_audioManager.failureSFX);
        StartCoroutine(_uiFeedback.ImageFade(arrestImage, effectDuration));
    }
    public void CriminalResult()
    {
        _audioManager.PlaySfx(_audioManager.successSFX);
        StartCoroutine(_uiFeedback.ImageFade(criminalImage, effectDuration));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !_resultTakenIn)
        {
            Debug.Log("Turning on trigger image");
            _inRange = true;
            EvilNpcSetup();
            triggerHud.SetActive(true);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player"&& !_resultTakenIn)
        {
            Debug.Log("Turning off trigger image");
            _inRange = false;
            triggerHud.SetActive(false);
        }
    }
}