using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPathManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FloorPathIndicator indicatorPrefab;
    [SerializeField] private MissionManager missionManager;

    [Header("Line Settings")]
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField] private Material lineMaterial;

    private readonly Dictionary<PlayerController, FloorPathIndicator> _indicators = new();
    private PlayerManager _playerManager;

    private IEnumerator Start()
    {
        yield return null;

        _playerManager = PlayerManager.Instance;

        foreach (var player in _playerManager.Players)
            SpawnIndicator(player);

        _playerManager.OnPlayerConnected += SpawnIndicator;
        _playerManager.OnReconnectTimerOut += RemoveIndicator;
    }

    private void SpawnIndicator(PlayerController player)
    {
        if (_indicators.ContainsKey(player)) return;

        FloorPathIndicator indicator = Instantiate(indicatorPrefab, Vector3.zero, Quaternion.identity, transform);
        indicator.Init(player, missionManager, lineColor, lineMaterial);
        _indicators[player] = indicator;
    }

    private void RemoveIndicator(PlayerController player)
    {
        if (!_indicators.TryGetValue(player, out var indicator)) return;
        if (indicator != null) Destroy(indicator.gameObject);
        _indicators.Remove(player);
    }

    private void OnDestroy()
    {
        if (_playerManager == null) return;
        _playerManager.OnPlayerConnected -= SpawnIndicator;
        _playerManager.OnReconnectTimerOut -= RemoveIndicator;
    }
}