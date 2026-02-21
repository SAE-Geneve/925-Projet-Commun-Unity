using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas _pauseCanvas;
    [SerializeField] private ScoreBoard scoreboard;
    
    public static UIManager Instance { get; private set; }
    
    private GameManager _gameManager;

    public void ShowPauseCanvas(bool state) => _pauseCanvas.gameObject.SetActive(state);
    
    
    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start() => _gameManager = GameManager.Instance;

    public void SetupScoreBoard() => scoreboard.SetScores();

    public void DisplayScoreBoard() => scoreboard.Show();
}
