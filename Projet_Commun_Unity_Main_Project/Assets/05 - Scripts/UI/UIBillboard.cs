using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Awake() => _cameraTransform = Camera.main.transform;
    
    private void Update() => transform.forward = _cameraTransform.forward;
}
