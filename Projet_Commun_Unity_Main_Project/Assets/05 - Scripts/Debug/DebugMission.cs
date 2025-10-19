using UnityEngine;

public class DebugMission : Mission
{
    [Header("Debug Mission")]
    [SerializeField] private Material _unlockedMaterial;
    [SerializeField] private Material _lockedMaterial;
    [SerializeField] private Material _playingMaterial;
    [SerializeField] private Material _finishedMaterial;
    
    private Renderer _renderer;

    protected override void Start()
    {
        _renderer = GetComponent<Renderer>();
        
        base.Start();
    }

    protected override void SwitchMissionState(MissionState newState)
    {
        base.SwitchMissionState(newState);

        switch (_missionState)
        {
            case MissionState.Unlocked: _renderer.material = _unlockedMaterial; break;
            case MissionState.Locked: _renderer.material = _lockedMaterial; break;
            case MissionState.Playing: _renderer.material = _playingMaterial; break;
            case MissionState.Finished: _renderer.material = _finishedMaterial; break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_missionState != MissionState.Finished && other.CompareTag("Player")) StartMission();
    }

    private void OnTriggerExit(Collider other)
    {
        if(_missionState == MissionState.Playing && other.CompareTag("Player")) Finish();
    }
}