using UnityEngine;

public class KartController : MonoBehaviour, IInteractable
{
    [Header("Settings")]
    [SerializeField] private float interactCooldown = 0.1f;
    [SerializeField] private Transform seatPosition;
    
    private KartMovement _kartMovement;
    
    private void Awake() => _kartMovement = GetComponent<KartMovement>();

    public void Interact(PlayerController playerController)
    {
        playerController.Input.SwitchCurrentActionMap("Kart");
        playerController.KartController = this;
        playerController.KartMovement = _kartMovement;
        
        playerController.OnEnterKart?.Invoke();
    }

    public void Exit(PlayerController playerController)
    {
        playerController.Input.SwitchCurrentActionMap("game");
        playerController.KartController = null;
        playerController.KartMovement = null;

        playerController.transform.position = transform.position + transform.right * 2f;
        
        playerController.OnExitKart?.Invoke();
    }
}