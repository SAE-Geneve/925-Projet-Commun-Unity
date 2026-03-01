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
        
        missionNameTmp.SetText(missions[randomIndex].name);
        missionNameAnimator.SetTrigger("Display");
    }

    public void OnMissionFinished() => StartCoroutine(HubTimeRoutine());

    private IEnumerator HubTimeRoutine()
    {
        yield return new WaitForSeconds(hubTime);
        UnlockRandomMission();
    }
}
