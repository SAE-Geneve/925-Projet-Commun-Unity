using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DynamicRope : MonoBehaviour
{
    public Transform startPoint; // Assign the Kart (rear bumper)
    public Transform endPoint;   // Assign the Spawner (front bumper)
    
    [Header("Settings")]
    public int resolution = 20; // Higher = smoother curve
    public float ropeLength = 5f; // Should match your Joint's Linear Limit
    public float slackFactor = 3f; // How much it droops

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = resolution;
    }

    void LateUpdate()
    {
        if (startPoint == null || endPoint == null) return;

        // 1. Calculate the mid-point between the two objects
        Vector3 midPoint = (startPoint.position + endPoint.position) / 2;

        // 2. Calculate distance to see how tight the rope is
        float currentDistance = Vector3.Distance(startPoint.position, endPoint.position);
        
        // 3. Determine how much the rope should droop
        // If distance is near ropeLength, droop is 0. If distance is 0, droop is max.
        float droopHeight = Mathf.Max(0, ropeLength - currentDistance) / slackFactor;

        // 4. Lower the mid-point to create the curve anchor
        midPoint.y -= droopHeight;

        // 5. Draw the Bezier Curve
        DrawQuadraticBezierCurve(startPoint.position, midPoint, endPoint.position);
    }

    void DrawQuadraticBezierCurve(Vector3 point0, Vector3 point1, Vector3 point2)
    {
        for (int i = 0; i < resolution; i++)
        {
            float t = i / (float)(resolution - 1);
            // B(t) = (1-t)^2 * P0 + 2(1-t)t * P1 + t^2 * P2
            Vector3 position = (1 - t) * (1 - t) * point0 + 
                               2 * (1 - t) * t * point1 + 
                               t * t * point2;
            
            line.SetPosition(i, position);
        }
    }
}