using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Catcher _catcher;
    private Ragdoll _ragdoll;
    
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _catcher = GetComponent<Catcher>();
        _ragdoll = GetComponent<Ragdoll>();
    }

    #region Input Events

    public void OnMove(InputAction.CallbackContext context) => _playerMovement.SetMovement(context.ReadValue<Vector2>());

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _playerMovement.Dash();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.started) _catcher.TryInteract();
    }
    
    public void OnRagdoll(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _ragdoll.RagdollOn();
        }
    }

    #endregion
}
