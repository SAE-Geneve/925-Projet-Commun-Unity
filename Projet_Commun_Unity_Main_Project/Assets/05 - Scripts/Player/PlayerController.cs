using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    [Header("References")]
    [SerializeField] private Transform cameraTarget;
    public TextMeshProUGUI playerNumberText;
    
    [Header("Events")] 
    public UnityEvent OnEnterKart;
    public UnityEvent OnExitKart;
    
    public PlayerInput Input { get; private set;}
    public InputManager InputManager { get; private set;}
    public KartController KartController { get; set; }
    public KartMovement KartMovement { get; set; }
    public KartPhysic KartPhysic { get; set; }
    
    public Transform CameraTarget => cameraTarget;
    
    public int Id { get; set; }

    protected override void Start()
    {
        Input = GetComponent<PlayerInput>();
        InputManager = GetComponent<InputManager>();
        
        PlayerManager playerManager = PlayerManager.Instance;
        if(playerManager) transform.parent = playerManager.transform;
        
        base.Start();
    }
    
    public void TryInteract()
    {
        if (InteractableGrabbed != null)
        {
            InteractableGrabbed.Interact(this);
            return;
        }
        
        TryAction<IInteractable>(interactable =>
        {
            interactable.Interact(this);
        });
    }

    public void TryEndInteract()
    {
        if (InteractableGrabbed != null)
            InteractableGrabbed.InteractEnd();
    }
}