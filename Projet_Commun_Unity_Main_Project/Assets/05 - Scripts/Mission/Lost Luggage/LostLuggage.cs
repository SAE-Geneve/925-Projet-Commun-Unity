using UnityEngine;

public class LostLuggage : MonoBehaviour
{
    
    public static LostLuggage Instance;

    [Header("Références")] 
    public Transform[] spawnPoints;
    public DropZone[] dropZones;
    public GameObject valisePrefab;

    private GameObject currentValise;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartNewRound();
    }

    public void StartNewRound()
    {
        foreach (DropZone zone in dropZones)
        {
            zone.SetActiveZone(false);
        }

        int randomZoneIndex = Random.Range(0, dropZones.Length);
        dropZones[randomZoneIndex].SetActiveZone(true);

        if (currentValise != null)
        {
            Destroy(currentValise);
        }

        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomSpawnIndex];
        
        currentValise = Instantiate(valisePrefab, selectedSpawnPoint.position, selectedSpawnPoint.rotation);
        
        // currentValise.AddComponent<ObjectPulsing>();
    }

    public void ValiseLivree()
    {
        StartNewRound();
    }
}
