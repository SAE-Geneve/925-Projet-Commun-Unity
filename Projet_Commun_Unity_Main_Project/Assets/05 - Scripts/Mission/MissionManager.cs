using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Mission> missions;
    [SerializeField] private TextMeshProUGUI missionNameTmp;
    [SerializeField] private Animator missionNameAnimator;
    
    public Mission currentMission;
    public int missionIndex;
    
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
        currentMission = missions[randomIndex];
        missionIndex=randomIndex;
        
        missionNameTmp.SetText(missions[randomIndex].Name);
        missionNameAnimator.SetTrigger("Display");
    }

    public void UnlockAllMissions()
    {
        foreach (Mission mission in missions)
            mission.Unlock();
    }

    public void OnMissionFinished() => StartCoroutine(HubTimeRoutine());
    public void OnOneMissionFinished() => StartCoroutine(OneMissionTimeRoutine());

    private IEnumerator HubTimeRoutine()
    {
        yield return new WaitForSeconds(hubTime);
        UnlockRandomMission();
    }

    private IEnumerator OneMissionTimeRoutine()
    {
        yield return new WaitForSeconds(hubTime);
        currentMission.Unlock();
    }
}
