using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas _pauseCanvas;
    [SerializeField] private Canvas _scoreBoardCanvas;

    [Header("Parameters")] 
    [SerializeField] private float scoreBoardTime = 5f;
    
    public static UIManager Instance { get; private set; }
    
    private GameManager _gameManager;

    public void ShowPauseCanvas(bool state) => _pauseCanvas.gameObject.SetActive(state);
    
    
    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start() => _gameManager = GameManager.Instance;

    public void DisplayScoreBoard()
    {
        if(_gameManager.State != GameState.Playing) return;
        StartCoroutine(ScoreBoardRoutine());
    }

    private IEnumerator ScoreBoardRoutine()
    {
        _gameManager.SwitchState(GameState.Cinematic);
        _scoreBoardCanvas.enabled = true;
        yield return new WaitForSeconds(scoreBoardTime);
        _scoreBoardCanvas.enabled = false;
        _gameManager.SwitchState(GameState.Playing);
    }
}
