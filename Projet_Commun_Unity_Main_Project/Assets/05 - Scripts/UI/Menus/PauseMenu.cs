using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Canvas pauseCanvas;

    void Start()
    {
        GameManager.Instance.SwitchState(GameState.Playing);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.State == GameState.Playing)
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseCanvas.enabled = true;
        GameManager.Instance.PauseTrigger();
    }

    public void ResumeGame()
    {
        pauseCanvas.enabled = false;
        GameManager.Instance.PauseTrigger();
    }
}
