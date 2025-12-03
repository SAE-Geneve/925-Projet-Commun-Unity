using Unity.Cinemachine;
using UnityEngine;

public class CameraSetter : MonoBehaviour
{
    private CinemachineCamera _cinemachineCamera;

    private void Start()
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    public void SetPriority(int value) => _cinemachineCamera.Priority = value;
}
