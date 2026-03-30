using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MissionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<Mission> missions;
    [SerializeField] private TextMeshProUGUI missionNameTmp;
    [SerializeField] private Animator missionNameAnimator;
    
    [SerializeField] private Mission currentMission;
    public Mission CurrentMission => currentMission;
    
    private int _missionIndex;
    public int MissionIndex => _missionIndex;
    
    public event Action OnMissionReady;
    public event Action OnMissionCooldownStarted;
    
    [Header("Parameters")]
    [SerializeField] private float hubTime = 30f;
    
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
        _missionIndex=randomIndex;
        
        missionNameTmp.SetText(missions[randomIndex].Name);
        missionNameAnimator.SetTrigger("Display");
        
        OnMissionReady?.Invoke();
    }

    public void UnlockAllMissions()
    {
        foreach (Mission mission in missions)
            mission.Unlock();
    }

    public void OnMissionFinished()
    { 
        OnMissionCooldownStarted?.Invoke();
        StartCoroutine(HubTimeRoutine());
    }
    public void OnOneMissionFinished()
    { 
        OnMissionCooldownStarted?.Invoke();
        StartCoroutine(OneMissionTimeRoutine());
    }

    private IEnumerator HubTimeRoutine()
    {
        yield return new WaitForSeconds(hubTime);
        UnlockRandomMission();
    }

    private IEnumerator OneMissionTimeRoutine()
    {
        yield return new WaitForSeconds(hubTime);
        currentMission.Unlock();
        OnMissionReady?.Invoke();
    }
}
