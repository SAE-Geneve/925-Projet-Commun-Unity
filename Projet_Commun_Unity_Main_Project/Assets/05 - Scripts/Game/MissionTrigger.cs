using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    [Header("References")] 
    [Tooltip("The mission to trigger")]
    [SerializeField] private Mission _mission;
    [Tooltip("The tmp of the state of the mission")]
    [SerializeField] private TextMeshProUGUI _stateTmp;
    [SerializeField] private TextMeshProUGUI _numberTmp;
    
    [SerializeField] private Material _unlockedMaterial;
    [SerializeField] private Material _lockedMaterial;

    [Header("Rules Display")]
    [Tooltip("Temps d'affichage des règles en secondes")]
    [SerializeField] private float _rulesDisplayTime = 10f;

    private Renderer _renderer;
    private PlayerManager _playerManager;
    private Action<Ragdoll> _ragdollHandler;
    private readonly HashSet<Ragdoll> _ragdolls = new();
    private bool _isSequenceStarted = false;
    private int _playerNumber;

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _playerManager = PlayerManager.Instance;

        if (_playerManager)
        {
            _playerManager.OnPlayerAdded += UpdateTmpNumber;
            _playerManager.OnPlayerRemoved += UpdateTmpNumber;
            _playerManager.OnPlayerRemoved += CheckPlayerNumber;
        }

        if (_mission)
        {
            _mission.OnSwitchState += UpdateState;
            UpdateState();
        }

        _ragdollHandler = ragdoll =>
        {
            Decrement();
            ragdoll.OnRagdollSelf -= _ragdollHandler;
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !other.TryGetComponent(out Ragdoll ragdoll)) return;

        if (_ragdolls.Add(ragdoll))
        {
            _playerNumber++;
            ragdoll.OnRagdollSelf += _ragdollHandler;
            UpdateTmpNumber();
            CheckPlayerNumber();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !other.TryGetComponent(out Ragdoll ragdoll)) return;

        if (_ragdolls.Remove(ragdoll))
        {
            ragdoll.OnRagdollSelf -= _ragdollHandler;
            Decrement();
        }
    }

    private void CheckPlayerNumber()
    {
        if (_playerManager && _playerNumber == _playerManager.PlayerCount && !_isSequenceStarted)
        {
            StartCoroutine(StartMissionSequence());
        }
    }

    private IEnumerator StartMissionSequence()
    {
        _isSequenceStarted = true;
        GameObject rulesInstance = null;

        Time.timeScale = 0f;

        if (_mission != null && _mission.ExplanationPrefab != null)
        {
            rulesInstance = Instantiate(_mission.ExplanationPrefab);
        }

        yield return new WaitForSecondsRealtime(_rulesDisplayTime);

        if (rulesInstance != null)
        {
            Destroy(rulesInstance);
        }

        Time.timeScale = 1f;
        StartMission();
    }

    private void StartMission()
    {
        foreach (var ragdoll in _ragdolls)
            if (ragdoll) ragdoll.OnRagdollSelf -= _ragdollHandler;

        _ragdolls.Clear();
        _playerNumber = 0;
        UpdateTmpNumber();
            
        _mission.StartMission();
    }

    private void Decrement()
    {
        _playerNumber--;
        if(_playerNumber < 0) _playerNumber = 0;
        UpdateTmpNumber();
    }

    private void UpdateTmpNumber()
    {
        if (_playerManager) _numberTmp.SetText($"{_playerNumber}/{_playerManager.PlayerCount}");
    }

    private void UpdateState()
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