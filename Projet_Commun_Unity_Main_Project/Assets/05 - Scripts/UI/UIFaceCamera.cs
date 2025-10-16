using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Awake() => _cameraTransform = Camera.main.transform;

    private void Update()
    {
        Quaternion lookRotation = _cameraTransform.transform.rotation;
        transform.rotation = lookRotation;
    }
}
