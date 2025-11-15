using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas pauseCanvas;
    private bool _isPaused;
    private Canvas HUDCanvas;

    void Start()
    {
        TryGetComponent(out HUDCanvas);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseCanvas.enabled = true;
        GameManager.Instance.SwitchState(GameState.Paused);
        _isPaused = true;
    }

    public void ResumeGame()
    {
        pauseCanvas.enabled = false;
        GameManager.Instance.SwitchState(GameState.Playing);
        _isPaused = false;
    }
}
