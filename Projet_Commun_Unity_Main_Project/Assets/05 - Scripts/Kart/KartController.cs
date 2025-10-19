using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class KartController : MonoBehaviour, IInteractable
{
    [NonSerialized]public bool IsControlled = false;
    private GameObject _currentDriver;
    private MonoBehaviour _driverInputHandler;
    
    private float _lastInteractTime;
    [SerializeField] private float interactCooldown = 0.1f;
    
    [Header("Input")]
    public Transform seatPosition;

    private KartMouvement _kartMouvement;
    private void Start()
    {
        _kartMouvement = GetComponent<KartMouvement>();        
    }

    public void OnInteract()
    {
        if (Time.time - _lastInteractTime < interactCooldown)
            return;

        _lastInteractTime = Time.time;
        
        if (IsControlled)
        {
            Debug.Log("EXIT");
            ExitVehicle();
        }
    }
    public void Interact(GameObject interactor)
    {   
        
        if (!IsControlled){
            Debug.Log("Enter");
            EnterVehicle(interactor);
        }
        else if (interactor == _currentDriver){
            Debug.Log("EXIT2");
            ExitVehicle();
        }
    }

    private void EnterVehicle(GameObject interactor)
    {
        _currentDriver = interactor;
        IsControlled = true;
        
        _driverInputHandler = interactor.GetComponentInChildren<MonoBehaviour>();
        if (_driverInputHandler != null)
            _driverInputHandler.enabled = false;

        interactor.SetActive(false);
        
        interactor.transform.position = seatPosition.position;
        _lastInteractTime = Time.time;
    }

    private void ExitVehicle()
    {
        if (_currentDriver == null) return;

        IsControlled = false;

        _currentDriver.SetActive(true);
        _currentDriver.transform.position = transform.position + transform.right * 2f;

        if (_driverInputHandler != null)
            _driverInputHandler.enabled = true;

        _kartMouvement.ResetInputs();
        _currentDriver = null;
        _driverInputHandler = null;
    }
}