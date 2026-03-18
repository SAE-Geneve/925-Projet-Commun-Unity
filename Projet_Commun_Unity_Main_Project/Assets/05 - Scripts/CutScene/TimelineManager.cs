using UnityEngine;
using UnityEngine.Playables;
using Unity.Cinemachine;

public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance { get; private set; }

    [Header("Directors")]
    [SerializeField] private PlayableDirector startDirector;
    [SerializeField] private PlayableDirector victoryDirector;
    [SerializeField] private PlayableDirector looseDirector;
    [SerializeField] private PlayableDirector endingDirector;

    [Header("Cameras")]
    [SerializeField] private CinemachineCamera timelineCamera;
    [SerializeField] private CinemachineCamera gameplayCamera;

    [Header("Options")]
    [SerializeField] private bool disablePlayerInputWhilePlaying = true;

    public bool IsPlaying { get; private set; }

    private PlayableDirector currentDirector;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        GameManager.Instance.SwitchState(GameState.Cinematic);
        PlayStart();
    }

    public void RegisterDirectors(
        PlayableDirector start = null,
        PlayableDirector victory = null,
        PlayableDirector loose = null,
        PlayableDirector ending = null)
    {
        if (start != null) startDirector = start;
        if (victory != null) victoryDirector = victory;
        if (loose != null) looseDirector = loose;
        if (ending != null) endingDirector = ending;
    }

    public void PlayStart()   => PlayDirector(startDirector);
    public void PlayVictory() => PlayDirector(victoryDirector);
    public void PlayLoose()   => PlayDirector(looseDirector);
    public void PlayEnding()  => PlayDirector(endingDirector);

    public void PlayResult(bool isVictory)
    {
        if (isVictory) PlayVictory();
        else PlayLoose();
    }

    public void PlayDirector(PlayableDirector director)
    {
        if (director == null)
        {
            Debug.Log("[TimelineManager] PlayDirector called but director is null.");
            GameManager.Instance.SwitchState(GameState.Playing);
            return;
        }

        Debug.Log($"[TimelineManager] Trying to play director: {director.name}");
        Debug.Log($"[TimelineManager] Playable asset: {director.playableAsset}");

        StopCurrentTimeline();

        currentDirector = director;
        BeginTimelineState();

        currentDirector.time = 0;
        currentDirector.extrapolationMode = DirectorWrapMode.None;
        currentDirector.stopped += OnDirectorStopped;
        
        Debug.Log($"[TimelineManager] timeUpdateMode = {currentDirector.timeUpdateMode}");
        Debug.Log($"[TimelineManager] duration = {currentDirector.duration}");
        Debug.Log($"[TimelineManager] initialTime = {currentDirector.time}");
        Debug.Log($"[TimelineManager] director enabled = {currentDirector.enabled}");
        Debug.Log($"[TimelineManager] director activeInHierarchy = {currentDirector.gameObject.activeInHierarchy}");
        currentDirector.Play();

        Debug.Log($"[TimelineManager] Director state after Play(): {currentDirector.state}");
    }

    private void BeginTimelineState()
    {
        IsPlaying = true;

        if (timelineCamera != null)
            timelineCamera.Priority = 100;

        if (gameplayCamera != null)
            gameplayCamera.Priority = 0;
    }

    private void EndTimelineState()
    {
        IsPlaying = false;

        if (timelineCamera != null)
            timelineCamera.Priority = 0;

        if (gameplayCamera != null)
            gameplayCamera.Priority = 100;

        GameManager.Instance.SwitchState(GameState.Playing);
    }

    private void OnDirectorStopped(PlayableDirector director)
    {
        director.stopped -= OnDirectorStopped;

        if (director != currentDirector)
            return;

        currentDirector = null;
        EndTimelineState();
    }

    public void StopAllTimelines()
    {
        StopDirector(startDirector);
        StopDirector(victoryDirector);
        StopDirector(looseDirector);
        StopDirector(endingDirector);

        currentDirector = null;
        IsPlaying = false;
    }

    private void StopCurrentTimeline()
    {
        if (currentDirector == null)
            return;

        currentDirector.stopped -= OnDirectorStopped;
        currentDirector.Stop();
        currentDirector = null;
    }

    private void StopDirector(PlayableDirector director)
    {
        if (director == null)
            return;

        director.stopped -= OnDirectorStopped;

        if (director.state == PlayState.Playing)
            director.Stop();
    }
}