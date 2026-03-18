using UnityEngine;

public class MainMenuReset : MonoBehaviour
{
    void OnEnable()
    {
        PlayerManager.Instance.Reset();
    }
}
