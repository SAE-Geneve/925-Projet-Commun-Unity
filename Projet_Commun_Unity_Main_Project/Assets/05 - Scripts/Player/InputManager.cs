using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerController _playerController;
    private KartController _kartController;
    
    public event Action<PlayerController> OnControllerDisconnected;
    
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerController = GetComponent<PlayerController>();
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
        else if(context.canceled) _playerController.TryEndInteract();
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
        if (context.canceled)
        {
            _playerController.KartMovement.ResetInputs();
        }
        _playerController.KartMovement.Move(context.ReadValue<Vector2>());
        _playerController.KartPhysic.Move(context.ReadValue<Vector2>());
    }

    public void OnKartExit(InputAction.CallbackContext context)
    {
        _playerController.KartController.Exit(_playerController);
    }

    #endregion

    public void ControllerDisconnected() => OnControllerDisconnected?.Invoke(_playerController);
}
