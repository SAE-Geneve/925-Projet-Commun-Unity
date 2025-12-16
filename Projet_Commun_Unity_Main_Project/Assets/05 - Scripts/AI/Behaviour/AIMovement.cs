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
    [Tooltip("Layer des autres IA pour l'évitement")]
    [SerializeField] private LayerMask avoidanceLayer; 

    [Header("Path Correction (Anti-Blocage)")]
    [Tooltip("Layer des Murs/Objets (Important pour recalculer si on est coincé derrière)")]
    [SerializeField] private LayerMask obstacleLayer; 
    [Tooltip("Intervalle de vérification du chemin (en secondes)")]
    [SerializeField] private float checkPathInterval = 0.2f;

    private NavMeshPath _path;
    private int _currentCorner;
    private bool _moving;
    private Vector3 _destination;
    
    private float _nextPathCheckTime;

    protected override void Start()
    {
        base.Start();
        _path = new NavMeshPath();
        Rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (!_moving) return;

        // --- 1. Vérification Path vide ---
        if (_path.corners.Length == 0 || _currentCorner >= _path.corners.Length)
        {
            Stop();
            return;
        }

        Vector3 agentPos = transform.position;
        Vector3 nextCorner = _path.corners[_currentCorner];
        Vector3 dirToCorner = nextCorner - agentPos;
        dirToCorner.y = 0f;

        // --- 2. NOUVEAU : Vérification de trajectoire ---
        // On vérifie si le chemin vers le prochain point est bloqué par un mur (suite à une poussette)
        if (Time.time >= _nextPathCheckTime)
        {
            _nextPathCheckTime = Time.time + checkPathInterval;
            
            // On lance un rayon vers le prochain coin
            // Si on touche un mur (ObstacleLayer), c'est qu'on a été décalé et qu'on ne peut plus passer tout droit.
            if (Physics.Raycast(agentPos + Vector3.up * 0.5f, dirToCorner.normalized, dirToCorner.magnitude, obstacleLayer))
            {
                // MUR DÉTECTÉ ! On recalcule le chemin depuis notre nouvelle position.
                SetDestination(_destination);
                return; // On attend la prochaine frame pour le nouveau chemin
            }
        }

        // --- 3. Passage au point suivant ---
        if (dirToCorner.magnitude <= 0.2f || (_currentCorner == _path.corners.Length - 1 && dirToCorner.magnitude <= stopDistance))
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

        // --- 4. Evitement simple (Boids) ---
        Vector3 avoidanceVector = GetSeparationVector();
        
        // --- 5. Mouvement Final ---
        Vector3 finalDirection = (dirToCorner + (avoidanceVector * avoidanceWeight)).normalized;
        MoveAI(finalDirection);
    }

    private Vector3 GetSeparationVector()
    {
        Vector3 separation = Vector3.zero;
        Collider[] neighbors = Physics.OverlapSphere(transform.position, avoidanceRadius, avoidanceLayer);
        int count = 0;

        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject == gameObject) continue;
            if (neighbor.isTrigger) continue;

            Vector3 pushDirection = transform.position - neighbor.transform.position;
            pushDirection.y = 0; 

            float distance = pushDirection.magnitude;
            if (distance < 0.01f) distance = 0.01f;

            float strength = 1.0f - (distance / avoidanceRadius); 
            if (strength > 0)
            {
                separation += pushDirection.normalized * strength;
                count++;
            }
        }

        if (count > 0) separation /= count;
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

        if (flatVelocity.sqrMagnitude > 0.1f && dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        }

        InvokeOnMove(dir);
    }

    public void SetDestination(Vector3 target)
    {
        _destination = target;
        // On recalcul le chemin depuis la position ACTUELLE
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
        {
            if (NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, _path))
            {
                _currentCorner = 0;
                // Petite astuce : si le point 0 est très proche (notre position), on vise direct le 1
                if (_path.corners.Length > 1 && Vector3.Distance(transform.position, _path.corners[0]) < 0.5f)
                {
                    _currentCorner = 1;
                }
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

    public bool HasReachedDestination() => !_moving;

    void OnDrawGizmosSelected()
    {
        // Visualisation de l'évitement
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, avoidanceRadius);

        // Visualisation du chemin
        if (_path != null && _path.corners.Length > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < _path.corners.Length - 1; i++)
                Gizmos.DrawLine(_path.corners[i], _path.corners[i + 1]);
            
            // Visualisation de la direction actuelle
            if (_currentCorner < _path.corners.Length)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, _path.corners[_currentCorner]);
            }
        }
    }
}