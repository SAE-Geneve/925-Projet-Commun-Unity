using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    public MissionID TargetMission { get; set; }
    
    [SerializeField] private Canvas _pauseCanvas;

    public void ShowPauseCanvas(bool state) => _pauseCanvas.gameObject.SetActive(state);
    
    
    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
}
