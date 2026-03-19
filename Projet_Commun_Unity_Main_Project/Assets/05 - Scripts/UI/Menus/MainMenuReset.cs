using UnityEngine;

public class MainMenuReset : MonoBehaviour
{
    void OnEnable()
    {
        if(PlayerManager.Instance)
            PlayerManager.Instance.Reset();
    }
}
