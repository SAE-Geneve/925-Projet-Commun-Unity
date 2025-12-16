using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class AIMovement : CharacterMovement
{
    [Header("AI Navigation")]
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float accelerationPower = 50f;

    [Header("Avoidance (Anti-Collision)")]
    [Tooltip("Rayon de détection des autres IA")]
    [SerializeField] private float avoidanceRadius = 2.0f; 
    [Tooltip("Puissance de la force de répulsion")]
    [SerializeField] private float avoidanceWeight = 1.5f; 
    [Tooltip("Quels layers doivent repousser l'IA (mettre le layer des IA/Player)")]
    [SerializeField] private LayerMask avoidanceLayer; 

    private NavMeshPath _path;
    private int _currentCorner;
    private bool _moving;
    private Vector3 _destination;

    protected override void Start()
    {
        base.Start();
        _path = new NavMeshPath();
        Rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (!_moving)
        {
            return;
        }

        if (_path.corners.Length == 0 || _currentCorner >= _path.corners.Length)
        {
            Stop();
            return;
        }

        Vector3 agentPos = transform.position;
        Vector3 nextCorner = _path.corners[_currentCorner];
        Vector3 dirToCorner = nextCorner - agentPos;
        dirToCorner.y = 0f;
        
        if (dirToCorner.magnitude <= 0.15f || (_currentCorner == _path.corners.Length - 1 && dirToCorner.magnitude <= stopDistance))
        {
            _currentCorner++;
            if (_currentCorner >= _path.corners.Length)
            {
                Stop();
                return;
            }

            nextCorner = _path.corners[_currentCorner];
            dirToCorner = nextCorner - agentPos;
            dirToCorner.y = 0f;
        }

        dirToCorner.Normalize();
        Vector3 avoidanceVector = GetSeparationVector();
        Vector3 finalDirection = (dirToCorner + (avoidanceVector * avoidanceWeight)).normalized;
        MoveAI(finalDirection);
    }
    private Vector3 GetSeparationVector()
    {
        Vector3 separation = Vector3.zero;
        
        Collider[] neighbors = Physics.OverlapSphere(transform.position, avoidanceRadius, avoidanceLayer);

        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject == gameObject) continue;
            
            if (neighbor.isTrigger) continue;

            Transform otherTransform = neighbor.transform;
            
            Vector3 pushDirection = transform.position - otherTransform.position;
            pushDirection.y = 0;
            
            float distance = pushDirection.magnitude;
            if (distance > 0.01f)
            {
                separation += pushDirection.normalized / distance;
            }
        }
        return separation;
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
        _destination = target;
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
        {
            if (NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, _path))
            {
                _currentCorner = 0;
                _moving = true;
            }
        }
    }

    public void Stop()
    {
        if (Rb.isKinematic) return;

        _moving = false;
        Rb.linearVelocity = Vector3.zero;
        Rb.angularVelocity = Vector3.zero;
    }

    public bool HasReachedDestination()
    {
        return !_moving;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);

        if (_path == null || _path.corners.Length == 0) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < _path.corners.Length - 1; i++)
            Gizmos.DrawLine(_path.corners[i], _path.corners[i + 1]);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_destination, 0.3f);
    }
}