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
        var playerRb = playerController.GetComponent<Rigidbody>();
        var playerCol = playerController.GetComponent<Collider>();
        
        if (playerRb) playerRb.isKinematic = true;
        if (playerCol) playerCol.enabled = false;
        
        Renderer[] playerRenderers = playerController.GetComponentsInChildren<Renderer>();
        foreach (var r in playerRenderers)
        {
            r.enabled = false;
        }
        
        playerController.transform.SetParent(seatPosition);
        //playerController.transform.position = seatPosition.position;
        playerController.transform.localPosition = Vector3.zero;
        playerController.transform.localRotation = Quaternion.identity;
        playerController.transform.localScale = Vector3.one;
        
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
        
        Renderer[] playerRenderers = playerController.GetComponentsInChildren<Renderer>();
        foreach (var r in playerRenderers)
        {
            r.enabled = true;
        }
        
        var playerRb = playerController.GetComponent<Rigidbody>();
        var playerCol = playerController.GetComponent<Collider>();
        
        if (playerRb) 
        {
            playerRb.isKinematic = false;
            playerRb.linearVelocity = Vector3.zero; 
            playerRb.angularVelocity = Vector3.zero;
        }
        if (playerCol) playerCol.enabled = true;
        
        playerController.transform.localScale = Vector3.one;
        
        playerController.Input.SwitchCurrentActionMap("game");
        playerController.KartController = null;
        playerController.KartMovement = null;

        playerController.transform.position = transform.position + transform.right * 2f;
        playerController.transform.rotation = Quaternion.identity;
        
        playerController.OnExitKart?.Invoke();
    }
}