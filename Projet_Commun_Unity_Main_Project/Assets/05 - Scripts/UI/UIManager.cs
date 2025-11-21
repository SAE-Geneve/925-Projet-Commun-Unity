using UnityEngine;

public class UIManager : MonoBehaviour
{
    
    public static UIManager Instance { get; private set; }
    
    [SerializeField] private Canvas pauseCanvas;

    public void ShowPauseCanvas(bool state)
    {
        pauseCanvas.enabled = state;
    }
    
    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
}
