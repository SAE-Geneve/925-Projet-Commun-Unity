using System;
using UnityEngine;

public class BorderControl : MonoBehaviour
{
    [SerializeField] private GameObject addedConveyor1;
    [SerializeField] private DirectionSwitch directionSwitch1;
    
    [SerializeField] private AIManagerBorder aiManager;
    [SerializeField] private float[] spawnInterval = new float[4];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        AdaptConveyorNumber();
    }

    public void AdaptConveyorNumber()
    {
        switch (PlayerManager.Instance.Players.Count)
        {
            case 1:
                addedConveyor1.SetActive(false);
                directionSwitch1.SetDirection(0);
                aiManager.SetSpawnInterval(spawnInterval[0]);
                break;
            case 2:
                addedConveyor1.SetActive(false);
                directionSwitch1.SetDirection(0);
                aiManager.SetSpawnInterval(spawnInterval[1]);
                break;
            case 3:
                addedConveyor1.SetActive(true);
                directionSwitch1.SetDirection(1);
                aiManager.SetSpawnInterval(spawnInterval[2]);
                break;
            case 4:
                addedConveyor1.SetActive(true);
                directionSwitch1.SetDirection(1);
                aiManager.SetSpawnInterval(spawnInterval[3]);
                break;
        }
    }
}
