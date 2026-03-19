using UnityEngine;

public class PlayerNavigation : MonoBehaviour
{
    private PathManager pathManager;
    private int playerID;

    void Start()
    {
        pathManager = FindObjectOfType<PathManager>();
        playerID    = gameObject.GetInstanceID();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MiniGameZone"))
            pathManager.RegisterPlayer(playerID, PathStage.Active);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MiniGameZone"))
            pathManager.UnregisterPlayer(playerID);
    }
}