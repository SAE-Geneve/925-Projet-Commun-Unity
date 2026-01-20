using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardingNPCUI : MonoBehaviour
{
    [Header("NPC UI")] [SerializeField] GameObject triggerHud;

    [Header("UI Slot Element")] public List<Sprite> passportPhotos;
    public Image passportSlot;

    [Header("UI Effect Elements")] [SerializeField]
    private Image happyImage;

    [SerializeField] private Image unhappyImage;
    [SerializeField] private Image criminalImage;
    [SerializeField] private Image arrestImage;

    //Bool values
    private bool _inRange = false;
    private bool _resultTakenIn = false;
    public bool _isEvil = false;

    //Components
    private RandomCharacterSkin randomSkin;
    private PlayerInput _playerInput;
    private AudioManager _audioManager;
    private UIFeedback _uiFeedback;

    [SerializeField] private float effectDuration = 3f;

    void Start()
    {
        randomSkin = transform.parent.GetComponent<RandomCharacterSkin>();
        if (TryGetComponent(out _uiFeedback))
        {
            Debug.Log("Found UI Text Effects");
            _uiFeedback.ImagePoolCreation(happyImage);
        }

        _audioManager = AudioManager.Instance;
        _playerInput = FindAnyObjectByType<PlayerInput>();
        
        if (_isEvil)
        {
            EvilNpcSetup();
        }
        else
        {
            GoodNpcSetup();
        }
    }

    void EvilNpcSetup()
    {
        while (passportSlot.sprite == null)
        {
            int evilIndex = Random.Range(0, passportPhotos.Count);
            if (randomSkin.randomIndex != evilIndex)
            {
                passportSlot.sprite = passportPhotos[evilIndex];
            }
        }
    }

    void GoodNpcSetup()
    {
        passportSlot.sprite = passportPhotos[randomSkin.randomIndex];
    }

    void Update()
    {
        if (_isEvil)
        {
            if (_inRange && _playerInput.actions["Interact"].triggered && !_resultTakenIn)
            {
                Debug.Log("Accepted");
                _resultTakenIn = true;
                triggerHud.SetActive(false);
                CriminalResult();
            }
            else if (_inRange && _playerInput.actions["Grab"].triggered && !_resultTakenIn)
            {
                Debug.Log("Deny");
                _resultTakenIn = true;
                triggerHud.SetActive(false);
                ArrestResult();
            }
        }
        else
        {
            if (_inRange && _playerInput.actions["Interact"].triggered && !_resultTakenIn)
            {
                Debug.Log("Accepted");
                _resultTakenIn = true;
                triggerHud.SetActive(false);
                HappyResult();
            }
            else if (_inRange && _playerInput.actions["Grab"].triggered && !_resultTakenIn)
            {
                Debug.Log("Deny");
                _resultTakenIn = true;
                triggerHud.SetActive(false);
                UnhappyResult();
            }
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

    public void CriminalResult()
    {
        _audioManager.PlaySfx(_audioManager.failureSFX);
        StartCoroutine(_uiFeedback.ImageFade(criminalImage, effectDuration));
    }

    public void ArrestResult()
    {
        _audioManager.PlaySfx(_audioManager.successSFX);
        StartCoroutine(_uiFeedback.ImageFade(arrestImage, effectDuration));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !_resultTakenIn)
        {
            Debug.Log("Turning on trigger image");
            _inRange = true;
            triggerHud.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && !_resultTakenIn)
        {
            Debug.Log("Turning off trigger image");
            _inRange = false;
            triggerHud.SetActive(false);
        }
    }
}