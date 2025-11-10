using UnityEngine;

public class KartController : MonoBehaviour, IInteractable
{
    [Header("Settings")] 
    [SerializeField] private Transform seatPosition;
    
    private KartMovement _kartMovement;
    private KartPhysic _kartPhysic;

    private void Awake()
    {
        _kartMovement = GetComponent<KartMovement>();
        _kartPhysic = GetComponent<KartPhysic>();
    } 

    public void Interact(PlayerController playerController)
    {
        playerController.transform.SetParent(seatPosition);
        playerController.transform.position = seatPosition.position;
        
        playerController.Input.SwitchCurrentActionMap("Kart");
        playerController.KartController = this;
        playerController.KartPhysic = _kartPhysic;
        playerController.KartMovement = _kartMovement;
        
        playerController.OnEnterKart?.Invoke();
    }
    
    public void InteractEnd(){}

    public void Exit(PlayerController playerController)
    {
        playerController.transform.SetParent(PlayerManager.Instance.transform);
        
        playerController.Input.SwitchCurrentActionMap("game");
        playerController.KartController = null;
        playerController.KartMovement = null;

        playerController.transform.position = transform.position + transform.right * 2f;
        
        playerController.OnExitKart?.Invoke();
    }
}