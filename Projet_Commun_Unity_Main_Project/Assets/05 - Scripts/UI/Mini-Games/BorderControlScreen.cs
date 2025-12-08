using UnityEngine;
using UnityEngine.UI;

public class BorderControlScreen : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Image[] imageSlots;
    [SerializeField] private Sprite[] goodItems;
    [SerializeField] private Sprite[] badItems;
    
    public void SetupScreen()
    {
        foreach (var slot in imageSlots)
            slot.sprite = goodItems[Random.Range(0, goodItems.Length)];
    }

    public void SetupBadScreen()
    {
        SetupScreen();
        int evilSlot = Random.Range(0, imageSlots.Length);
        imageSlots[evilSlot].sprite = badItems[Random.Range(0, badItems.Length)];
    }
}
