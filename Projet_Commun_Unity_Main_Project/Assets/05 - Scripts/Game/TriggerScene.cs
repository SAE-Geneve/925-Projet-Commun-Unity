using System;
using TMPro;
using UnityEngine;

public class TriggerScene : MonoBehaviour
{
    [Header("References")] 
    [Tooltip("The tmp of the state of the mission")]
    [SerializeField] private TextMeshProUGUI _stateTmp;

    [Tooltip("The tmp that shows the number of player in the trigger box")]
    [SerializeField] private TextMeshProUGUI _numberTmp;
    
    [SerializeField] private Material _unlockedMaterial;
    [SerializeField] private Material _lockedMaterial;
    
    [Header("Parameters")]
    [Tooltip("The linked mission")]
    [SerializeField] private MissionID _missionID = MissionID.BorderControl;
    
    private Renderer _renderer;
    private Mission _mission;
    private PlayerManager _playerManager;
    
    private Action<Ragdoll> _ragdollHandler;
    
    private int _playerNumber;

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _mission = GameManager.Instance.GetMission(_missionID);
        _playerManager = PlayerManager.Instance;

        _playerManager.OnPlayerAdded += UpdateTmpNumber;
        _playerManager.OnPlayerRemoved += UpdateTmpNumber;
        _playerManager.OnPlayerRemoved += CheckPlayerNumber;
        
        InitState();

        _ragdollHandler = ragdoll =>
        {
            Decrement();
            ragdoll.OnRagdollSelf -= _ragdollHandler;
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !other.TryGetComponent(out Ragdoll ragdoll)) return;

        _playerNumber++;
        
        CheckPlayerNumber();
        UpdateTmpNumber();

        ragdoll.OnRagdollSelf += _ragdollHandler;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !other.TryGetComponent(out Ragdoll ragdoll)) return;

        ragdoll.OnRagdollSelf -= _ragdollHandler;

        Decrement();
    }

    private void CheckPlayerNumber()
    {
        if(_playerNumber == _playerManager.PlayerCount)
            _mission.StartMission();
    }

    private void Decrement()
    {
        _playerNumber--;
        UpdateTmpNumber();
    }

    private void UpdateTmpNumber() => _numberTmp.SetText($"{_playerNumber}/{_playerManager.PlayerCount}");

    private void InitState()
    {
        if (_mission.IsLocked)
        {
            _renderer.material = _lockedMaterial;
            _stateTmp.SetText("Locked");
            _stateTmp.color = Color.red;

        }
        else
        {
            _renderer.material = _unlockedMaterial;
            _stateTmp.SetText("Unlocked");
            _stateTmp.color = Color.green;
        }
        
        UpdateTmpNumber();
    }
}