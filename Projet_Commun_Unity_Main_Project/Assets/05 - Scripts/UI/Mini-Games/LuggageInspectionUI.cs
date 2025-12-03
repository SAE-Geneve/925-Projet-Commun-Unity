using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuggageInspectionUI : MonoBehaviour
{
    [Header("Luggage UI")]
    [SerializeField] GameObject triggerHud;
    
    [Header("UI Slots Elements")]
    public List<Sprite> badItems;
    public List<Sprite> goodItems;
    public List<Image> slots;
    
    //Components
    private AudioManager _audioManager;
    
    //Randomness for slots setup
    private int _random;
    private int _randomEvilSlot;
    
    void Start()
    {
        _audioManager = AudioManager.Instance;
        
        EvilLuggageSetup();
    }
    
    void EvilLuggageSetup()
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

    void GoodLuggageSetup()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            _random = Random.Range(0, goodItems.Count);
            slots[i].sprite = goodItems[_random];
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LuggageScanner")
        {
            Debug.Log("Turning on trigger image");
            triggerHud.SetActive(true);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "LuggageScanner")
        {
            Debug.Log("Turning off trigger image");
            triggerHud.SetActive(false);
        }
    }
}
