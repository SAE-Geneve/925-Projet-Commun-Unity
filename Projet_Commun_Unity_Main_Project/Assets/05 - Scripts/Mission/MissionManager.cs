using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

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
    public bool missionCooldown;
    
    public float GetHubTime() => hubTime;
    public static MissionManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }
    
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

    public void OnMissionFinished()
    { 
        missionCooldown = true;
        StartCoroutine(HubTimeRoutine());
    }
    public void OnOneMissionFinished()
    { 
        missionCooldown = true;
        StartCoroutine(OneMissionTimeRoutine());
    }

    private IEnumerator HubTimeRoutine()
    {
        yield return new WaitForSeconds(hubTime);
        missionCooldown = false;
        UnlockRandomMission();
    }

    private IEnumerator OneMissionTimeRoutine()
    {
        yield return new WaitForSeconds(hubTime);
        missionCooldown = false;
        currentMission.Unlock();
    }
}
