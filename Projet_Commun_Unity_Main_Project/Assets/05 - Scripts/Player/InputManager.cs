using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerController _playerController;
    private KartController _kartController;
    private Ragdoll _ragdoll;
    
    public event Action<PlayerController> OnControllerDisconnected;
    
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerController = GetComponent<PlayerController>();
        _ragdoll = GetComponent<Ragdoll>();
    }

    #region Player Input Events

    public void OnMove(InputAction.CallbackContext context) => _playerMovement.SetMovement(context.ReadValue<Vector2>());

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started) _playerMovement.Dash();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.started) _playerController.TryInteract();
    }
    
    public void OnRagdoll(InputAction.CallbackContext context)
    {
        if (context.started) _ragdoll.RagdollOn();
    }

    public void OnCatch(InputAction.CallbackContext context)
    {
        if (context.started)
            _playerController.CatchStart();
        else if (context.canceled)
            _playerController.CatchCanceled();
    }

    #endregion

    #region Kart Input Events

    public void OnKartMove(InputAction.CallbackContext context)
    {
        _playerController.KartMovement.OnMove(context);
    }

    public void OnKartExit(InputAction.CallbackContext context)
    {
        _playerController.KartController.Exit(_playerController);
    }

    #endregion

    public void ControllerDisconnected() => OnControllerDisconnected?.Invoke(_playerController);
}
