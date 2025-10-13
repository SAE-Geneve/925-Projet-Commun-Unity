using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class AIMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stopDistance = 0f;

    private Rigidbody rb;
    private Vector3 destination;
    private NavMeshPath path;
    private int currentCorner = 0;
    private bool moving = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        path = new NavMeshPath();
    }

    void FixedUpdate()
    {
        if (!moving) return;

        if (path.corners.Length == 0 || currentCorner >= path.corners.Length)
        {
            Stop();
            return;
        }

        Vector3 agentPos = transform.position;
        Vector3 nextCorner = path.corners[Mathf.Min(currentCorner, path.corners.Length - 1)];
        Vector3 dir = nextCorner - agentPos;
        dir.y = 0f;

        // Ajouter une petite tolérance pour éviter le mini teleport
        if (dir.magnitude <= stopDistance || dir.magnitude <= 0.05f)
        {
            currentCorner++;
            if (currentCorner >= path.corners.Length)
            {
                Stop();
                return;
            }
        }
        else
        {
            dir.Normalize();
            Move(dir);
        }
    }


    private void Move(Vector3 dir)
    {
        Vector3 moveDir = dir * speed;
        Vector3 velocityChange = moveDir - new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        // Rotation
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(dir), 0.15f);
        }
    }

    public void SetDestination(Vector3 target)
    {
        destination = target;
        if (NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path))
        {
            currentCorner = 0;
            moving = true;
        }
    }

    public void Stop()
    {
        moving = false;
        rb.linearVelocity = Vector3.zero;      // Reset complet de la vélocité
        rb.angularVelocity = Vector3.zero; // Supprime toute rotation résiduelle
    }


    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public bool HasReachedDestination()
    {
        return !moving;
    }

    // Debug draw
    void OnDrawGizmos()
    {
        if (path == null || path.corners.Length == 0) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(destination, 0.3f);
    }
}
