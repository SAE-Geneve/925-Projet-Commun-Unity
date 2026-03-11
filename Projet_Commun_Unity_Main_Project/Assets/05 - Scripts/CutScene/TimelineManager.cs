using UnityEngine;
using UnityEngine.Playables;
using System;
using System.Collections;
public class TimelineManager : MonoBehaviour
{
    public static TimelineManager Instance {get; private set; }
    
    [Header ("Directors")]
    [SerializeField] private PlayableDirector startDirector;
    [SerializeField] private PlayableDirector victoryDirector;
    [SerializeField] private PlayableDirector looseDirector;
    [SerializeField] private PlayableDirector endingDirector;
    
    [Header ("Options")]
    [SerializeField] private bool disablePlayerInputWhilePlaying = true;
    
    public bool IsPlaying { get; private set; }

    public void RegisterDirectors(PlayableDirector start = null, PlayableDirector victory= null, PlayableDirector loose= null,
        PlayableDirector ending= null)
    {
        if (start   != null) startDirector   = start;
        if (victory != null) victoryDirector = victory;
        if (loose   != null) looseDirector   = loose;
        if (ending  != null) endingDirector  = ending;
    }

    private void Awake()
    {
        if (Instance !=null && Instance != this)
        {
            Destroy(gameObject);
            
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }
    public Coroutine PlayStart(Action onFinished = null)   => PlayDirector(startDirector, onFinished);
    public Coroutine PlayVictory(Action onFinished = null) => PlayDirector(victoryDirector, onFinished);
    public Coroutine PlayLoose(Action onFinished = null)   => PlayDirector(looseDirector, onFinished);
    public Coroutine PlayEnding(Action onFinished = null)  => PlayDirector(endingDirector, onFinished);
    
    public Coroutine PlayResult(bool isVictory, Action onFinished = null)=> isVictory ? PlayVictory(onFinished) : PlayLoose(onFinished);
    private Coroutine PlayDirector(PlayableDirector director, Action onFinished)
    {
        if (director == null)
        {
            Debug.LogWarning("[TimelineManager] PlayDirector called but director is null.");
            onFinished?.Invoke();
            return null;
        }

        return StartCoroutine(PlayRoutine(director, onFinished));
    }
    private IEnumerator PlayRoutine(PlayableDirector director, Action onFinished)
    {
        // Si une timeline est déjà en cours, on la stop.
        StopAllTimelines();

        IsPlaying = true;
        if (disablePlayerInputWhilePlaying) SetGameplayInput(false);

        bool done = false;

        void OnStopped(PlayableDirector d)
        {
            if (d == director) done = true;
        }

        director.time = 0;
        director.stopped += OnStopped;
        director.Play();

        // Attend la fin
        while (!done)
            yield return null;

        director.stopped -= OnStopped;

        if (disablePlayerInputWhilePlaying) SetGameplayInput(true);
        IsPlaying = false;

        onFinished?.Invoke();
    }

    public void StopAllTimelines()
    {
        StopDirector(startDirector);
        StopDirector(victoryDirector);
        StopDirector(looseDirector);
        StopDirector(endingDirector);
        IsPlaying = false;
    }

    private void StopDirector(PlayableDirector d)
    {
        if (d != null && d.state == PlayState.Playing)
            d.Stop();
    }
    private void SetGameplayInput(bool enabled)
    {
        // Exemples possibles :
        // - désactiver un PlayerController
        // - désactiver un InputActionAsset
        // - mettre GameManager en pause input
    }
}
