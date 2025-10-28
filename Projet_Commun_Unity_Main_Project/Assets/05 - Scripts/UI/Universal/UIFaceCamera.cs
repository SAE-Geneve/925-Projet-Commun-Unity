using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    private Transform _cameraTransform;

    private void Awake() => _cameraTransform = Camera.main.transform;

    private void Update()
    {
        transform.rotation = _cameraTransform.transform.rotation;
    }
}
