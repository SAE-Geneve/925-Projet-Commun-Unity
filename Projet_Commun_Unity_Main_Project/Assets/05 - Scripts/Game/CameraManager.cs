using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CinemachineCamera _cam;
    [SerializeField] private PlayerManager _playerManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cam = GetComponent<CinemachineCamera>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
