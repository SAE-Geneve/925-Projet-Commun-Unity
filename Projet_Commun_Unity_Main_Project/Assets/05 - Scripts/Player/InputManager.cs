using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerController _controller;
    private Ragdoll _ragdoll;
    
    public event Action<PlayerController> OnControllerDisconnected;
    
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _controller = GetComponent<PlayerController>();
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
        if(context.started) _controller.TryInteract();
    }
    
    public void OnRagdoll(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _ragdoll.RagdollOn();
        }
    }

    public void OnCatch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _controller.CatchStart();
        }
        else if (context.canceled)
        {
            _controller.CatchCanceled();
        }
    }

    #endregion

    public void ControllerDisconnected()
    {
        OnControllerDisconnected?.Invoke(_controller);
    }
}
