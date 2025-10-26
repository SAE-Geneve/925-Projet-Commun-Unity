using UnityEngine;

public class TimedMission : Mission
{
    [Header("Timed Mission")]
    [SerializeField] private float _initialTimer = 60f;

    public float Timer
    {
        get => _timer;
        private set
        {
            if (_timer < 0) Finish();
            _timer = value;
        }
    }

    private float _timer;

    protected override void OnStartMission()
    {
        base.OnStartMission();
        
        Timer = _initialTimer;
    }

    private void Update()
    {
        if(_gameManager.State == GameState.Playing && _missionState == MissionState.Playing)
            Timer -= Time.deltaTime;
    }
}