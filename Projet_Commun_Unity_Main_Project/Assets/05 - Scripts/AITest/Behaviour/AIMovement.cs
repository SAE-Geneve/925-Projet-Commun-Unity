using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class AIMovementTest : CharacterMovement
{
    [Header("AI Navigation")]
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float accelerationPower = 50f;
    
    private NavMeshPath path;
    private int currentCorner = 0;
    private bool moving = false;
    private Vector3 destination;

    protected override void Start()
    {
        base.Start();
        path = new NavMeshPath();
        Rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (!moving)
        {
            if (_animator != null)
                _animator.SetFloat("Speed", 0f);
            return;
        }

        if (path.corners.Length == 0 || currentCorner >= path.corners.Length)
        {
            Stop();
            if (_animator != null)
                _animator.SetFloat("Speed", 0f);
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
                if (_animator != null)
                    _animator.SetFloat("Speed", 0f);
                return;
            }

            nextCorner = path.corners[currentCorner];
            dirToCorner = nextCorner - agentPos;
            dirToCorner.y = 0f;
        }

        dirToCorner.Normalize();
        MoveAI(dirToCorner);

        if (_animator != null)
        {
            Vector3 flatVelocity = new Vector3(Rb.linearVelocity.x, 0, Rb.linearVelocity.z);
            _animator.SetFloat("Speed", flatVelocity.magnitude / speed);
        }
    }

    private void MoveAI(Vector3 dir)
    {
        Rb.AddForce(dir * accelerationPower, ForceMode.Acceleration);

        Vector3 flatVelocity = new Vector3(Rb.linearVelocity.x, 0, Rb.linearVelocity.z);
        if (flatVelocity.magnitude > speed)
        {
            Rb.linearVelocity = flatVelocity.normalized * speed + new Vector3(0, Rb.linearVelocity.y, 0);
        }

        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.15f);
        }

        InvokeOnMove(dir);
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
        if(Rb.isKinematic) return;
        
        moving = false;
        Rb.linearVelocity = Vector3.zero;
        Rb.angularVelocity = Vector3.zero;
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
