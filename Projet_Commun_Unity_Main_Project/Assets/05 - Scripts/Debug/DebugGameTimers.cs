using TMPro;
using UnityEngine;

public class DebugGameTimers : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _timerTmp;
    [SerializeField] private TextMeshProUGUI _disconnectionTimerTmp;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;

        _gameManager.OnTimerUpdate += UpdateTimer;
        _gameManager.OnDisconnectionTimerUpdate += UpdateDisconnectionTimer;
    }
    
    private void UpdateTimer() => _timerTmp.SetText($"Global Timer : {FormatTime(_gameManager.Timer)}");
    private void UpdateDisconnectionTimer() => _disconnectionTimerTmp.SetText($"Disconnection Timer : {FormatTime(_gameManager.DisconnectionTimer)}");
    
    private string FormatTime(float timeInSeconds)
    {
        int minutes = (int)(timeInSeconds / 60);
        int seconds = (int)(timeInSeconds % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}
