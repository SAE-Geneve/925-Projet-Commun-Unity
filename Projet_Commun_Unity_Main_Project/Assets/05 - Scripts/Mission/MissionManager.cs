using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Mission> missions;
    
    [Header("Parameters")]
    [SerializeField] private float hubTime = 30f;

    private void Start() => UnlockRandomMission();

    public void UnlockRandomMission()
    {
        if (missions.Count <= 0)
        {
            Debug.LogWarning("Random mission size is 0");
            return;
        }
        
        int randomIndex = Random.Range(0, missions.Count);
        
        missions[randomIndex].Unlock();
    }

    public void OnMissionFinished() => StartCoroutine(HubTimeRoutine());

    private IEnumerator HubTimeRoutine()
    {
        yield return new WaitForSeconds(hubTime);
        UnlockRandomMission();
    }
}
