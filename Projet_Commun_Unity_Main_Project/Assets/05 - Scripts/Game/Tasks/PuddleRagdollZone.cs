using UnityEngine;

public class PuddleRagdollZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ragdoll ragdoll)) ragdoll.RagdollOn();
    }
}