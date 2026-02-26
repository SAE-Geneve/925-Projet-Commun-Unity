using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIPlayerShowcase : MonoBehaviour
{
    [SerializeField] private bool fixedSize;

    [Header("Images")]
    [SerializeField] private TextMeshProUGUI[] playerNames;
    [SerializeField] private Image[] playerBoxImages;
    [SerializeField] private Image[] playerIconImages;
    [SerializeField] private Sprite playerIcon;
    [SerializeField] private Sprite missingIcon;

    [Header("Colors")]
    [SerializeField] private Color[] playerBoxColors;
    [SerializeField] private Color[] playerIconColors;
    [SerializeField] private Color missingBoxColor;
    [SerializeField] private Color missingIconColor;

    void Update()
    {
        for (int i = 0; i < PlayerManager.Instance.PlayerCount; i++)
        {
            if (!fixedSize)
            {
                RectTransform rt = playerIconImages[i].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, 335);
            }
            playerIconImages[i].sprite = playerIcon;
            playerIconImages[i].color = playerIconColors[i];
            playerBoxImages[i].color = playerBoxColors[i];
            playerNames[i].color = playerIconColors[i];
        }

        for (int i = PlayerManager.Instance.PlayerCount; i < 4; i++)
        {
            if(!fixedSize)
            {
                RectTransform rt = playerIconImages[i].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.x);
            }
            playerIconImages[i].sprite = missingIcon;
            playerIconImages[i].color = missingIconColor;
            playerBoxImages[i].color = missingBoxColor;
            playerNames[i].color = missingIconColor;
        }
    }
}
