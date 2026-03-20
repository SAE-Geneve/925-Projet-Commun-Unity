using System;
using System.Collections;
using System.Collections.Generic;
using PlasticGui;
using TMPro;
using UnityEngine;

public class NewMissionTrigger : MonoBehaviour
{
    [Header("References")] 
    [Tooltip("The mission to trigger")]
    [SerializeField] private Mission _mission;
    [Tooltip("The tmp of the state of the mission")]
    [SerializeField] private TextMeshProUGUI _stateTmp;
    [SerializeField] private TextMeshProUGUI _numberTmp;
    [SerializeField] private float _scrollSpeed;
    
    [SerializeField] private Color _unlockedColor;
    [SerializeField] private Color _lockedColor;

    private PlayerManager _playerManager;
    private Action<Ragdoll> _ragdollHandler;
    private readonly HashSet<Ragdoll> _ragdolls = new();
    private bool _isSequenceStarted = false;
    private int _playerNumber;
    private AudioManager _audioManager;
    
    private Material _edgeMaterial;


    private void Start()
    {
        _audioManager = AudioManager.Instance;
        _playerManager = PlayerManager.Instance;
        
        var renderers = GetComponentsInChildren<Renderer>();                                                                                                                                                                                                              
        if (renderers.Length > 0)                                                                                                                                                                                                                                           {
            _edgeMaterial = renderers[0].material; // creates a unique instance for this trigger                                                                                                                                                                          
            foreach (var r in renderers)                                                                                                                                                                                                                                            r.material = _edgeMaterial; // all 4 quads share that same instance
        }         
        Debug.Log($"Found {renderers.Length} renderers");

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

    private void Update()
    {
        if (_edgeMaterial)
            _edgeMaterial.mainTextureOffset -= new Vector2(_scrollSpeed * Time.deltaTime, 0);   
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _playerNumber++;
        UpdateTmpNumber();
        CheckPlayerNumber();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Decrement();
    }

    private void CheckPlayerNumber()
    {
        if (_playerManager && _playerNumber == _playerManager.PlayerCount && !_isSequenceStarted)
        {
            StartMissionSequence();
        }
    }

    private void StartMissionSequence()
    {
        _isSequenceStarted = true;
        
        Time.timeScale = 0f;

        if (_mission != null && _mission.ExplanationPrefab != null)
        {
            GameObject rulesInstance = Instantiate(_mission.ExplanationPrefab);
            
            var uiScript = rulesInstance.GetComponent<MissionExplanationUI>();

            if (uiScript != null)
            {
                uiScript.OnContinueClicked += () => 
                {
                    Destroy(rulesInstance);
                    Time.timeScale = 1f;
                    StartMission();
                };
            }
            else
            {
                Debug.LogError("Le script MissionExplanationUI est manquant sur le prefab !");
                Destroy(rulesInstance);
                Time.timeScale = 1f;
                StartMission();
            }
        }
        else
        {
            Time.timeScale = 1f;
            StartMission();
        }
    }

    private void StartMission()
    {
        _audioManager.PlaySfx(_audioManager.StartMissionSFX);
        
        foreach (var ragdoll in _ragdolls)
            if (ragdoll) ragdoll.OnRagdollSelf -= _ragdollHandler;

        _ragdolls.Clear();
        _playerNumber = 0;
        UpdateTmpNumber();
            
        _mission.StartMission();
        _isSequenceStarted = false;
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
            _edgeMaterial.color = _lockedColor;
            _stateTmp.SetText("Locked");
            _stateTmp.color = Color.red;
        }
        else
        {
            _edgeMaterial.color = _unlockedColor;
            _stateTmp.SetText("Unlocked");
            _stateTmp.color = Color.green;
        }
        UpdateTmpNumber();
    }
}