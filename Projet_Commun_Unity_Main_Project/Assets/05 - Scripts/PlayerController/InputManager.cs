using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private CharacterMovement _characterMovement;

    private void Start()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    public void OnMove(InputAction.CallbackContext context) => _characterMovement.MoveInput = context.ReadValue<Vector2>();
    public void OnJump(InputAction.CallbackContext context) => _characterMovement.IsJumping = context.ReadValueAsButton();
}
