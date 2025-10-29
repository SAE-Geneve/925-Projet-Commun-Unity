using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCUIInteraction : MonoBehaviour
{
    [SerializeField] GameObject triggerHud;
    
    [Header("UI Elements")]
    public List<Sprite> badItems;
    public List<Sprite> goodItems;
    public List<Image> slots;
    
    private bool _inRange = false;
    private PlayerInput playerInput;
    
    private int _random;

    void Start()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();
        
        _random = Random.Range(0, goodItems.Count);
        slots[0].sprite = goodItems[_random];
        
        _random = Random.Range(0, goodItems.Count);
        slots[1].sprite = goodItems[_random];
        
        _random = Random.Range(0, goodItems.Count);
        slots[2].sprite = goodItems[_random];
    }

    void Update()
    {
        if (_inRange && playerInput.actions["Interact"].triggered)
        {
            Debug.Log("Interact");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Turning on trigger image");
            _inRange = true;
            triggerHud.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Turning off trigger image");
            _inRange = false;
            triggerHud.SetActive(false);
        }
    }
}