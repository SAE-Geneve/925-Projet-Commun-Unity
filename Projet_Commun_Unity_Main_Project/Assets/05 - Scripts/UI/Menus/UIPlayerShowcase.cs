using UnityEngine;
using UnityEngine.UI;

public class UIPlayerShowcase : MonoBehaviour
{
    [SerializeField] private bool fixedSize;
    [SerializeField] private Image[] playerBoxes;

    [Header("Images")]
    [SerializeField] private Sprite playerIcon;
    [SerializeField] private Sprite missingIcon;

    [Header("Colors")]
    [SerializeField] private Color playerColor;
    [SerializeField] private Color missingColor;

    void Update()
    {
        for (int i = 0; i < PlayerManager.Instance.PlayerCount; i++)
        {
            if (!fixedSize)
            {
                RectTransform rt = playerBoxes[i].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, 335);
            }
            playerBoxes[i].sprite = playerIcon;
            playerBoxes[i].color = playerColor;
        }

        for (int i = PlayerManager.Instance.PlayerCount; i < 4; i++)
        {
            if(!fixedSize)
            {
                RectTransform rt = playerBoxes[i].GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.x);
            }
            playerBoxes[i].sprite = missingIcon;
            playerBoxes[i].color = missingColor;
        }
    }
}
