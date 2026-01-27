using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PernamentTimerUI : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Image backdropScore;
    
    [Header("Colors")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    private float _totalTime;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _totalTime = _gameManager.Timer;
    }
    
    private void Update()
    {
        int minutes = Mathf.FloorToInt(_gameManager.Timer / 60f);
        int seconds = Mathf.FloorToInt(_gameManager.Timer % 60f);
    
        timeText.SetText($"{minutes:00}:{seconds:00}");
        
        float t = 1f - Mathf.Clamp01(_gameManager.Timer / _totalTime);
        backdropScore.color = Color.Lerp(startColor, endColor, t);
    }
}
