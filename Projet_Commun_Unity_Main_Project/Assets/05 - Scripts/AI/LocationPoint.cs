using UnityEngine;

public class LocationPoint : MonoBehaviour
{
    [Header("Parameters")]
    public bool available = true;
    [SerializeField] private Vector3 forward;

    [Header("Gizmo Settings")]
    [SerializeField] private Color gizmoColor = Color.green;
    [SerializeField] private float gizmoSize = 0.5f;
    
    public Vector3 Forward => forward;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = available ? gizmoColor : Color.red;
        Gizmos.DrawSphere(transform.position, gizmoSize);
    }
}