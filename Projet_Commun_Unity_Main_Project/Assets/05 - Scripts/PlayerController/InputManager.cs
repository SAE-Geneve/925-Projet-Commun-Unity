using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public void OnMove(InputAction.CallbackContext context) => _playerMovement.SetMovement(context.ReadValue<Vector2>());

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _playerMovement.Dash();
        }
    }
}
