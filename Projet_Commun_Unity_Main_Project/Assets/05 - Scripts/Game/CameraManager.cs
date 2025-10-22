using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CinemachineCamera _cam;
    
    [SerializeField] private float minFOV = 60f;
    [SerializeField] private float maxFOV = 80f;
    [SerializeField] private float zoomMultiplier = 1.5f;
    
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Transform targetPosition;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cam = GetComponent<CinemachineCamera>();
        _cam.Lens.FieldOfView = minFOV;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerManager.Players.Count <= 0) return;
        targetPosition.position = GetMeanVector();

        if (minFOV + GetSpacing() <= maxFOV)
        {
            _cam.Lens.FieldOfView = minFOV + GetSpacing() * zoomMultiplier;
        }
    }

    private float GetSpacing()
    {
        float spacing = 0;
        int playerCount = playerManager.Players.Count;
        
        foreach (var pl in playerManager.Players)
        {
            Vector3 pos = pl.transform.position;
            
            spacing += Mathf.Abs((pos.x - targetPosition.position.x)) + Mathf.Abs((pos.z - targetPosition.position.z));
        }
        return spacing / playerCount;
    }
    
    private Vector3 GetMeanVector(){
        int playerCount = playerManager.Players.Count;
        
        float x = 0f;
        float y = 0f;
        float z = 0f;
 
        foreach (var pl in playerManager.Players)
        {
            Vector3 pos = pl.transform.position;
            x += pos.x;
            y += pos.y;
            z += pos.z;
        }
        return new Vector3(x / playerCount, y / playerCount, z / playerCount);
    }
}
