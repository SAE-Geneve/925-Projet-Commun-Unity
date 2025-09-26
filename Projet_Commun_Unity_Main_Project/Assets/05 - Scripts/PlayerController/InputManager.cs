using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Vector2 _moveInput = Vector2.zero;
    private bool _jumpInput;

    public Vector2 MoveInput => _moveInput;
    public bool JumpInput => _jumpInput;

    public void OnMove(InputAction.CallbackContext context) => _moveInput = context.ReadValue<Vector2>();
    public void OnJump(InputAction.CallbackContext context) => _jumpInput = context.ReadValueAsButton();
}
