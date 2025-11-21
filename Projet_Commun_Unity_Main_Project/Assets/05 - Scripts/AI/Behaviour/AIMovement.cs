using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
public class AIMovementTest : CharacterMovement
{
    [Header("AI Navigation")]
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float accelerationPower = 50f;
    
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
            if (_animator) _animator.SetFloat("Speed", 0f);
            return;
        }

        if (_path.corners.Length == 0 || _currentCorner >= _path.corners.Length)
        {
            Stop();
            if (_animator) _animator.SetFloat("Speed", 0f);
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
                if (_animator) _animator.SetFloat("Speed", 0f);
                return;
            }

            nextCorner = _path.corners[_currentCorner];
            dirToCorner = nextCorner - agentPos;
            dirToCorner.y = 0f;
        }

        dirToCorner.Normalize();
        MoveAI(dirToCorner);

        if (_animator)
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
        _destination = target;
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
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
        if(Rb.isKinematic) return;
        
        _moving = false;
        Rb.linearVelocity = Vector3.zero;
        Rb.angularVelocity = Vector3.zero;
    }

    public bool HasReachedDestination()
    {
        return !_moving;
    }

    // Debug draw
    void OnDrawGizmos()
    {
        if (_path == null || _path.corners.Length == 0) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < _path.corners.Length - 1; i++)
            Gizmos.DrawLine(_path.corners[i], _path.corners[i + 1]);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_destination, 0.3f);
    }
}
