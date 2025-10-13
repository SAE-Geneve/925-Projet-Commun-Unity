using UnityEngine;

public class DebugSphereCheck : MonoBehaviour
{
    public Transform checkPoint;
    public float radius = 5f;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(checkPoint.position, radius);
        foreach (var hit in hits)
        {
            Debug.Log("Hit détecté : " + hit.name);
        }
    }

    private void OnDrawGizmos()
    {
        if (!checkPoint) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(checkPoint.position, radius);
    }
}