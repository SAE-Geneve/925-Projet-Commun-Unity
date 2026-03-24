using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class DirectorTest : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private CinemachineCamera cutsceneCamera;

    private void Start()
    {
        director.played += d => Debug.Log($"PLAYED {d.name} t={d.time}");
        director.paused += d => Debug.Log($"PAUSED {d.name} t={d.time}");
        director.stopped += d =>
        {
            Debug.Log($"STOPPED {d.name} t={d.time}");
            cutsceneCamera.Priority = 0;
        };

        Debug.Log($"asset={director.playableAsset}");
        Debug.Log($"duration={director.duration}");
        Debug.Log($"mode={director.timeUpdateMode}");
        Debug.Log($"enabled={director.enabled}");
        Debug.Log($"active={director.gameObject.activeInHierarchy}");

        director.time = 0;
        director.Play();

        Debug.Log($"state after play={director.state}");
    }
}