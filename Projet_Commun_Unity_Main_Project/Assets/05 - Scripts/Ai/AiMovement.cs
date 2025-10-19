using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class AIMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float accelerationPower = 50f;
    [SerializeField] private float turnSpeed = 0.15f;

    private Rigidbody rb;
    private Animator animator;
    private Vector3 destination;
    private NavMeshPath path;
    private int currentCorner = 0;
    private bool moving = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        path = new NavMeshPath();
        rb.freezeRotation = true; 
    }

    void FixedUpdate()
    {
        if (!moving)
        {
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }
    
        if (path.corners.Length == 0 || currentCorner >= path.corners.Length)
        {
            Stop();
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }

        Vector3 agentPos = transform.position;
        Vector3 nextCorner = path.corners[currentCorner];
        Vector3 dirToCorner = nextCorner - agentPos;
        dirToCorner.y = 0f;

        if (dirToCorner.magnitude <= 0.15f || (currentCorner == path.corners.Length - 1 && dirToCorner.magnitude <= stopDistance))
        {
            currentCorner++;

            if (currentCorner >= path.corners.Length)
            {
                Stop();
                if (animator != null) animator.SetFloat("Speed", 0f);
                return;
            }

            nextCorner = path.corners[currentCorner];
            dirToCorner = nextCorner - agentPos;
            dirToCorner.y = 0f;
        }

        dirToCorner.Normalize();
        Move(dirToCorner);
        
        if (animator != null)
        {
            Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            animator.SetFloat("Speed", flatVelocity.magnitude / speed);
        }
    }

    private void Move(Vector3 dir)
    {
        rb.AddForce(dir * accelerationPower, ForceMode.Acceleration);

        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        if (flatVelocity.magnitude > speed)
        {
            rb.linearVelocity = flatVelocity.normalized * speed + new Vector3(0, rb.linearVelocity.y, 0);
        }

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(dir), turnSpeed);
        }
    }

    public void SetDestination(Vector3 target)
    {
        destination = target;
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            if (NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, path))
            {
                currentCorner = 0;
                moving = true;
            }
        }
    }

    public void Stop()
    {
        moving = false;
        rb.linearVelocity = Vector3.zero;      
        rb.angularVelocity = Vector3.zero; 
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