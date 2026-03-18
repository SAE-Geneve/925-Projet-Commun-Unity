using UnityEngine;
using UnityEngine.UI;

public class UILobbyWarning : MonoBehaviour
{
    [SerializeField] private GameObject warningText;
    [SerializeField] private Button continueButton;

    void OnEnable()
    {
        continueButton.interactable = false;
        warningText.SetActive(true);
    }
    void Update()
    {
        if (PlayerManager.Instance.Players.Count != 0)
        {
            warningText.SetActive(false);
            continueButton.interactable = true;
        }
    }
}
