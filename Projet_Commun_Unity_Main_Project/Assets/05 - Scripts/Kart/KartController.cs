using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class KartController : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private float interactCooldown = 0.1f;
    [SerializeField] private Transform seatPosition;

    private GameObject _driver;
    private PlayerInput _playerInput;
    private MonoBehaviour _driverMovementScript;
    private KartMouvement _kartMouvement;
    private float _lastInteractTime;
    
    // Références pour la gestion dynamique des inputs
    private InputAction _moveAction; 
    private InputAction _interactAction; 
    
    // Références pour la gestion de l'affichage du joueur
    private Collider _driverCollider;
    private Renderer _driverRenderer;
    private GameObject _driverModel; 
    
    public bool IsControlled { get; private set; }

    private void Awake()
    {
        _kartMouvement = GetComponent<KartMouvement>();
    }

    public void Interact(GameObject interactor)
    {
        if (Time.time - _lastInteractTime < interactCooldown)
            return;

        _lastInteractTime = Time.time;

        if (!IsControlled)
            Enter(interactor);
        // L'action de sortie sera gérée par l'Input System dans OnKartInteract, pas ici
    }
    
    private void OnKartInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (IsControlled && _driver != null)
        {
            Exit();
        }
    }

    private void Enter(GameObject interactor)
    {
        _driver = interactor;
        IsControlled = true;

        _playerInput = interactor.GetComponent<PlayerInput>();
        // Assurez-vous d'avoir le bon type de script de mouvement du joueur ici
        _driverMovementScript = interactor.GetComponentInChildren<MonoBehaviour>(); 
        
        // --- GESTION VISUELLE ET PHYSIQUE DU JOUEUR ---
        
        // Désactiver le Collider principal (évite les collisions avec le kart)
        _driverCollider = interactor.GetComponent<Collider>();
        if (_driverCollider != null)
            _driverCollider.enabled = false;
            
        // Désactiver le Renderer (le modèle)
        _driverRenderer = interactor.GetComponentInChildren<Renderer>();
        if (_driverRenderer != null)
        {
            _driverRenderer.enabled = false;
        }
        else
        {
            // S'il est sur un enfant : CHANGER "VotreNomDuModele"
            _driverModel = interactor.transform.Find("VotreNomDuModele")?.gameObject; 
            if (_driverModel != null)
            {
                _driverModel.SetActive(false);
            }
        }
        
        // --- GESTION DES INPUTS ---

        if (_playerInput != null)
        {
            _playerInput.SwitchCurrentActionMap("Kart");
            
            var kartActionMap = _playerInput.currentActionMap;
            if (kartActionMap != null)
            {
                // Câblage de l'action de Mouvement (Move)
                _moveAction = kartActionMap.FindAction("Move"); 
                if (_moveAction != null)
                {
                    _moveAction.performed += _kartMouvement.OnMove;
                    _moveAction.canceled += _kartMouvement.OnMove;
                }
                
                // Câblage de l'action d'Interaction/Sortie (Interact)
                _interactAction = kartActionMap.FindAction("Interact"); 
                if (_interactAction != null)
                {
                    _interactAction.performed += OnKartInteract;
                }
            }
        }

        // Désactive les contrôles de mouvement du joueur (son script de mouvement)
        if (_driverMovementScript != null)
            _driverMovementScript.enabled = false;

        // Place le joueur sur le siège
        interactor.transform.position = seatPosition.position;

        Debug.Log($"{interactor.name} est entré dans {gameObject.name}");
    }

    private void Exit()
    {
        if (_driver == null) return;
        
        // --- DÉSABONNEMENT DES INPUTS (TRÈS IMPORTANT) ---
        if (_moveAction != null)
        {
            _moveAction.performed -= _kartMouvement.OnMove;
            _moveAction.canceled -= _kartMouvement.OnMove;
        }
        if (_interactAction != null)
        {
            _interactAction.performed -= OnKartInteract;
        }

        // Switch action map vers Player
        if (_playerInput != null)
            _playerInput.SwitchCurrentActionMap("Game");
            
        _kartMouvement.ResetInputs(); 

        // --- RÉACTIVATION VISUELLE ET PHYSIQUE DU JOUEUR ---
        
        // Réactiver le Collider
        if (_driverCollider != null)
            _driverCollider.enabled = true;
            
        // Réactiver le Renderer ou le modèle
        if (_driverRenderer != null)
        {
            _driverRenderer.enabled = true;
        }
        else if (_driverModel != null)
        {
            _driverModel.SetActive(true);
        }
        
        // Réactive le joueur et le déplace à côté du kart
        _driver.transform.position = transform.position + transform.right * 2f;

        // Réactive les contrôles de mouvement du joueur
        if (_driverMovementScript != null)
            _driverMovementScript.enabled = true;

        Debug.Log($"{_driver.name} est sorti de {gameObject.name}");

        IsControlled = false;
        
        // Réinitialisation des champs pour éviter les références nulles
        _driver = null;
        _playerInput = null;
        _driverMovementScript = null;
        _moveAction = null;
        _interactAction = null;
        _driverCollider = null;
        _driverRenderer = null;
        _driverModel = null;
    }
}