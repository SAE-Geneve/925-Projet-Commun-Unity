using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUBTriggerUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI playersNeededText;
    [SerializeField] private Image backdropReference;
    
    [Header("Images")]
    [SerializeField] private Sprite missingPlayerSprite;
    [SerializeField] private Sprite enoughPlayerSprite;
    
    public void UpdatePlayerCount(int playerCount, int playersNeeded)
    {
        playersNeededText.text = playerCount+"/"+playersNeeded;
        if (playersNeeded >= playerCount)
        {
            backdropReference.sprite = enoughPlayerSprite;
        }
        else
        {
            backdropReference.sprite = missingPlayerSprite;
        }
    }
}
