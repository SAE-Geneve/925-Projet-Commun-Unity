using UnityEngine;

public class UIBillboard : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Awake() => _cameraTransform = Camera.main.transform;
    
    private void LateUpdate()
    {
        Vector3 direction = transform.position - _cameraTransform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
