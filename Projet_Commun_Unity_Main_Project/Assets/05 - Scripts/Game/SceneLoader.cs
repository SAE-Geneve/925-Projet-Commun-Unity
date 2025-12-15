using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Debug.LogWarning(Instance.gameObject + " " + Instance.transform.parent);
            Destroy(gameObject);
        }
        else Instance = this;
    }

    public void LoadScene(string sceneName)
    {
        List<PlayerController> playerControllers = PlayerManager.Instance.Players;
        
        foreach (var player in playerControllers)
            player.Drop();

        SceneManager.LoadScene(sceneName);
    }
}