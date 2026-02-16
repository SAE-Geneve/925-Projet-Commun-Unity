using UnityEngine;

public class RigidbodyDebugger : MonoBehaviour
{
    private Rigidbody _rb;
    private AIMovement _aiMovement;

    [Header("Debug Settings")]
    public bool logVelocitySpikes = true;
    public float spikeThreshold = 10f; // Vitesse considérée comme "anormale"

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _aiMovement = GetComponent<AIMovement>();
    }

    void FixedUpdate()
    {
        if (_rb == null) return;

        // 1. Visualisation de la vélocité (Ligne VERTE)
        // Plus la ligne est longue, plus il va vite.
        Debug.DrawRay(transform.position, _rb.linearVelocity, Color.green);

        // 2. Détection de pics de vitesse (Le moment exact où ça part en vrille)
        if (logVelocitySpikes && _rb.linearVelocity.magnitude > spikeThreshold)
        {
            Debug.LogError($"[PHYSIQUE] Vitesse anormale détectée : {_rb.linearVelocity.magnitude} m/s | Direction : {_rb.linearVelocity.normalized}");
            
            // Qui est coupable ?
            if (_aiMovement != null && _aiMovement.enabled)
            {
                Debug.LogWarning(" -> AIMovement est ACTIF et pousse peut-être.");
                // Optionnel : On peut voir où il essaye d'aller
                Debug.DrawLine(transform.position, _aiMovement.GetDestination(), Color.yellow);
            }
            else
            {
                Debug.LogWarning(" -> AIMovement est ÉTEINT. C'est une force purement physique (Collision/Explosion).");
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // 3. Détection de conflits de collision (Ligne ROUGE)
        // Si l'IA touche le sol ou ses propres membres, on le voit.
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.red);
        }
    }
    
    // Pour dessiner l'état dans l'éditeur
    void OnGUI()
    {
        if (_rb == null) return;
        GUI.color = Color.black;
        GUILayout.Label($"Vel: {_rb.linearVelocity.magnitude:F2}");
        GUILayout.Label($"Kinematic: {_rb.isKinematic}");
        GUILayout.Label($"Movement Script: {(_aiMovement && _aiMovement.enabled ? "ON" : "OFF")}");
    }
}