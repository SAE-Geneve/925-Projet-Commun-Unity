using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main; 
    }

    void LateUpdate()
    {
        if (_mainCamera != null)
        {
            // Le Canvas prend exactement la même rotation que la caméra
            transform.rotation = _mainCamera.transform.rotation;
        }
    }
}