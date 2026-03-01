using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MissionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Mission> missions;
    [SerializeField] private Mission _finalMission;

    public void UnlockRandomMission()
    {
        if (missions.Count <= 0)
        {
            Debug.LogWarning("Random mission size is 0");
            return;
        }
        
        int randomIndex = Random.Range(0, missions.Count);
        
        missions[randomIndex].Unlock();
        missions.RemoveAt(randomIndex);
    }

    public void TryUnlockFinalMission()
    {
        if (missions.Count > 0) UnlockRandomMission();
        else _finalMission.Unlock();
    }
}
