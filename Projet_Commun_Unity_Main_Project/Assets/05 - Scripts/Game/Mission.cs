using UnityEngine;

public class Mission : MonoBehaviour
{
    // [Header("References")]
    // [Tooltip("All the tasks to do to complete the mission")]
    // [SerializeField] private GameTask[] _tasks;
    
    [Header("Parameters")] 
    [SerializeField] private string _name = "New Mission";
    [SerializeField] private bool _locked;
    
    GameManager _gameManager;
    
    private bool _missionPlaying;

    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void StartMission()
    {
        if (_locked)
        {
            Debug.LogWarning("Mission locked, cannot start mission");
            return;
        }
        
        _gameManager.StartMission(this);
    }

    public void OnMissionBegin()
    {
        Debug.Log($"Mission {_name} began");
    }

    public void Finish()
    {
        Debug.Log($"Mission {_name} finished");
    }
}
