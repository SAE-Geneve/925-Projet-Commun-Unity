using UnityEngine;

public class ConveyorBeltMission : MonoBehaviour
{
    [SerializeField] private AIManagerConveyorBelt aiManager;
    [SerializeField] private float[] spawnIntervals = new float[4];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AdaptMission();
    }


    public void AdaptMission()
    {
        switch (PlayerManager.Instance.PlayerCount)
        {
            case 1:
                aiManager.SetSpawnInterval(spawnIntervals[0]);
                break;
            case 2:
                aiManager.SetSpawnInterval(spawnIntervals[1]);
                break;
            case 3:
                aiManager.SetSpawnInterval(spawnIntervals[2]);
                break;
            case 4:
                aiManager.SetSpawnInterval(spawnIntervals[3]);
                break;
        }
    }
}
