using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BorderControlScreen : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Image[] imageSlots;
    [SerializeField] private Sprite[] goodItems;
    [SerializeField] private Sprite[] badItems;
    
    private Dictionary<Prop, Sprite[]> _propMemory = new Dictionary<Prop, Sprite[]>();
    
    public void DisplayScreenForProp(Prop prop)
    {
        if (_propMemory.ContainsKey(prop))
        {
            ApplySpritesToScreen(_propMemory[prop]);
            return;
        }
        
        Sprite[] newContent = new Sprite[imageSlots.Length];
        
        if (prop.Type == PropType.GoodProp) newContent = GenerateGoodContent();
        else if (prop.Type == PropType.BadProp) newContent = GenerateBadContent();
        _propMemory.Add(prop, newContent);
        prop.OnDestroyed += ForgetProp;
        
        ApplySpritesToScreen(newContent);
    }
    private Sprite[] GenerateGoodContent()
    {
        Sprite[] sprites = new Sprite[imageSlots.Length];
        for (int i = 0; i < sprites.Length; i++)
            sprites[i] = goodItems[Random.Range(0, goodItems.Length)];
        return sprites;
    }

    private Sprite[] GenerateBadContent()
    {
        Sprite[] sprites = GenerateGoodContent();
        int evilSlot = Random.Range(0, sprites.Length);
        sprites[evilSlot] = badItems[Random.Range(0, badItems.Length)];
        return sprites;
    }

    private void ApplySpritesToScreen(Sprite[] sprites)
    {
        for (int i = 0; i < imageSlots.Length; i++)
            imageSlots[i].sprite = sprites[i];
    }
    private void ForgetProp(Prop destroyedProp)
    {
        if (_propMemory.ContainsKey(destroyedProp))
        {
            _propMemory.Remove(destroyedProp);
            destroyedProp.OnDestroyed -= ForgetProp;
        }
    }
}