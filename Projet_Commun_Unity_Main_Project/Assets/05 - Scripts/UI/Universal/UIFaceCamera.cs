using UnityEngine;

public class UIFollowAndFaceCamera : MonoBehaviour
{
    [Header("Réglages du Ballon")]
    [Tooltip("L'objet qui tourne (Le chien)")]
    [SerializeField] private Transform _target; 
    
    [Tooltip("La distance vers le haut (La ficelle)")]
    [SerializeField] private Vector3 _offset = new Vector3(0, 2f, 0); 

    private Transform _cameraTransform;

    private void Awake() 
    {
        _cameraTransform = Camera.main.transform;
    }

    // On utilise LateUpdate plutôt que Update pour éviter les tremblements !
    private void LateUpdate()
    {
        // Sécurité : si on a oublié de mettre une cible, on arrête tout
        if (_target == null) return;

        // 1. Position : On se place SUR la cible, et on ajoute notre décalage vers le haut
        transform.position = _target.position + _offset;

        // 2. Rotation : On regarde toujours la caméra
        transform.rotation = _cameraTransform.rotation;
    }
}