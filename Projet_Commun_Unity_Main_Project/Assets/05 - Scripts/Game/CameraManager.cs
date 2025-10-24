using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // [SerializeField] private float minFOV = 60f;
    // [SerializeField] private float maxFOV = 80f;
    // [SerializeField] private float zoomMultiplier = 1.5f;
    // private Transform _targetPosition;
    
    [SerializeField] CinemachineCamera soloCam;
    [SerializeField] CinemachineCamera groupCam;
    private CinemachineBrain _camBrain;

    private PlayerManager _playerManager;
    private CinemachineTargetGroup _cineMachineTargetGroup;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerManager = PlayerManager.Instance;
        _cineMachineTargetGroup = GetComponent<CinemachineTargetGroup>();
        _camBrain = Camera.main.GetComponent<CinemachineBrain>();

        foreach (var pl in _playerManager.Players)
        {
            AddPlayerToTargetGroup(pl);
        }

        _playerManager.OnPlayerConnected += AddPlayerToTargetGroup;
        _playerManager.OnReconnectTimerOut += RemovePlayerFromTargetGroup;
        
        // _targetPosition = _playerManager.TrackingTarget;
        //
        // _cam = GetComponent<CinemachineCamera>();
        // _cam.Target.TrackingTarget = _targetPosition;
        // _cam.Lens.FieldOfView = minFOV;
    }

    void AddPlayerToTargetGroup(PlayerController player)
    {
        CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target();
        target.Object = player.transform;
        _cineMachineTargetGroup.Targets.Add(target);
        
        if (soloCam.Follow == null)
        {
            soloCam.Follow = player.transform;
        }

        UpdateCamPriority();
    }
    
    void RemovePlayerFromTargetGroup(PlayerController player)
    {
        foreach (var target in _cineMachineTargetGroup.Targets)
        {
            if (target.Object == player.transform)
            {
                _cineMachineTargetGroup.Targets.Remove(target);
                break;
            }
        }
        
        if (soloCam.Follow == player.transform)
        {
            soloCam.Follow = _cineMachineTargetGroup.Targets[0].Object;
        }

        UpdateCamPriority();
    }

    void UpdateCamPriority()
    {
        if (_cineMachineTargetGroup.Targets.Count <= 1)
        {
            soloCam.Priority = 1;
            groupCam.Priority = 0;
        }
        else
        {
            groupCam.Priority = 1;
            soloCam.Priority = 0;
        }
    }
    
    // // Update is called once per frame
    // void FixedUpdate()
    // {
    //     if (_playerManager.Players.Count <= 0) return;
    //     _targetPosition.position = GetMeanVector();
    //
    //     if (minFOV + GetSpacing() <= maxFOV)
    //     {
    //         _cam.Lens.FieldOfView = minFOV + GetSpacing() * zoomMultiplier;
    //     }
    // }
    //
    // private float GetSpacing()
    // {
    //     float spacing = 0;
    //     int playerCount = _playerManager.Players.Count;
    //     
    //     foreach (var pl in _playerManager.Players)
    //     {
    //         Vector3 pos = pl.transform.position;
    //         
    //         spacing += Mathf.Abs((pos.x - _targetPosition.position.x)) + Mathf.Abs((pos.z - _targetPosition.position.z));
    //     }
    //     return spacing / playerCount;
    // }
    //
    // private Vector3 GetMeanVector(){
    //     int playerCount = _playerManager.Players.Count;
    //     
    //     float x = 0f;
    //     float y = 0f;
    //     float z = 0f;
    //
    //     foreach (var pl in _playerManager.Players)
    //     {
    //         Vector3 pos = pl.transform.position;
    //         x += pos.x;
    //         y += pos.y;
    //         z += pos.z;
    //     }
    //     return new Vector3(x / playerCount, y / playerCount, z / playerCount);
    // }
}
