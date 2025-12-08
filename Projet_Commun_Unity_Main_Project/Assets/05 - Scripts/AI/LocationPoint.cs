using System;
using UnityEngine;

public class LocationPoint : MonoBehaviour
{
    public GameObject locationObject;
    public bool available = true;

    [Header("Gizmo Settings")]
    public Color gizmoColor = Color.green;
    public float gizmoSize = 0.5f;

    private void Start()
    {
        locationObject = gameObject;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = available ? gizmoColor : Color.red;
        Gizmos.DrawSphere(transform.position, gizmoSize);
    }
}