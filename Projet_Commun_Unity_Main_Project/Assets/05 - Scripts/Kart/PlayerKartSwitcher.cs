using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKartSwitcher : MonoBehaviour
{
    public GameObject player;
    public GameObject kart;
    public Transform seatPosition;

    // Scripts ou composants qui gèrent les inputs
    public MonoBehaviour playerInputHandler; // celui qui utilise UnityEvent
    public MonoBehaviour vehicleInputHandler; // celui qui reçoit SendMessage

    private bool isInVehicle = false;

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!isInVehicle && Vector3.Distance(player.transform.position, kart.transform.position) < 3f)
            {
                EnterVehicle();
            }
            else if (isInVehicle)
            {
                ExitVehicle();
            }
        }
    }

    void EnterVehicle()
    {
        isInVehicle = true;
        player.SetActive(false);
        player.transform.position = seatPosition.position;

        // Désactiver le joueur et activer le véhicule
        playerInputHandler.enabled = false;
        vehicleInputHandler.enabled = true;
    }

    void ExitVehicle()
    {
        isInVehicle = false;
        player.SetActive(true);
        player.transform.position = kart.transform.position + kart.transform.right * 2f;

        // Activer le joueur et désactiver le véhicule
        playerInputHandler.enabled = true;
        vehicleInputHandler.enabled = false;
    }
}