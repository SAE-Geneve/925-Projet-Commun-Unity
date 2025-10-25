using System;
using UnityEngine;

public class TriggerScene : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("The linked mission")]
    [SerializeField] private MissionID _missionID = MissionID.BorderControl;
    
    private Mission _mission;
    
    private Action<Ragdoll> _ragdollHandler;
    
    private int _playerNumber;

    private void Start()
    {
        _mission = GameManager.Instance.GetMission(_missionID);
        
        PlayerManager.Instance.OnPlayerRemoved += CheckPlayerNumber;

        _ragdollHandler = ragdoll =>
        {
            _playerNumber--;
            ragdoll.OnRagdollSelf -= _ragdollHandler;
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !other.TryGetComponent(out Ragdoll ragdoll)) return;

        _playerNumber++;
        CheckPlayerNumber();

        ragdoll.OnRagdollSelf += _ragdollHandler;
        
        Debug.Log("Enter");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !other.TryGetComponent(out Ragdoll ragdoll)) return;

        ragdoll.OnRagdollSelf -= _ragdollHandler;
        
        _playerNumber--;
        Debug.Log("Exit");
    }

    private void CheckPlayerNumber()
    {
        if(_playerNumber == PlayerManager.Instance.PlayerCount)
            _mission.StartMission();
    }
}