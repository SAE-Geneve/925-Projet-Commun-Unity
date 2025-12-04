using UnityEngine;

public class UILobbyWarning : MonoBehaviour
{
    void Update()
    {
        if (PlayerManager.Instance.Players.Count != 0)
        {
            gameObject.SetActive(false);
        }
    }
}
