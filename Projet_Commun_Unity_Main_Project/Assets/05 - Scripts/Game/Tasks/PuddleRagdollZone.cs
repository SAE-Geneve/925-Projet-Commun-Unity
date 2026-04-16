using System.Collections;
using UnityEngine;

public class PuddleRagdollZone : MonoBehaviour
{
    [SerializeField] private float waitBeforeRagdoll = 0.5f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ragdoll ragdoll))
        {
            StartCoroutine(WaitBeforeRagdoll(ragdoll));
        }
    }

    private IEnumerator WaitBeforeRagdoll(Ragdoll ragdoll)
    {
        yield return new WaitForSeconds(waitBeforeRagdoll);
        ragdoll.RagdollOn();
    }
}