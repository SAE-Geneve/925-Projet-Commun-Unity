using UnityEngine;

public class RagdollDebug : MonoBehaviour
{
    public void RagdollAllPlayers()
    {
        foreach (var player in PlayerManager.Instance.Players)
            player.GetComponent<Ragdoll>().RagdollOn();
    }
}
