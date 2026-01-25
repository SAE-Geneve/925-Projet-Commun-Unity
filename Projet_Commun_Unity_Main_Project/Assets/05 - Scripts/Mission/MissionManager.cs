using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Mission> _randomMissions;
    [SerializeField] private Mission _finalMission;

    public void UnlockRandomMission()
    {
        if (_randomMissions.Count <= 0)
        {
            Debug.LogWarning("Random mission size is 0");
            return;
        }
        
        int randomIndex = Random.Range(0, _randomMissions.Count);
        
        _randomMissions[randomIndex].Unlock();
        _randomMissions.RemoveAt(randomIndex);
    }

    public void TryUnlockFinalMission()
    {
        if (_randomMissions.Count > 0) UnlockRandomMission();
        else _finalMission.Unlock();
    }
}
