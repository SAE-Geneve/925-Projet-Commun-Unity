using UnityEngine;

public class PuddleTask : GameTask
{
    [Header("Score Reward (Hub Only)")]
    [SerializeField] private bool givePointsOnClean = false;
    [SerializeField] private int cleanReward = 50;

    [Header("Growth Settings")]
    [SerializeField] private bool canGrow = false;
    [SerializeField] private float maxScale = 3f;
    [SerializeField] private float growDuration = 15f;

    [Header("Puddle References")]
    [SerializeField] private Transform _visualTransform;

    private MopProp _mop;

    private float _cleanTimer;
    private float _effectiveCleanTime;
    private bool _isCleaning;

    private float _growTimer = 0f;
    private Vector3 _initialVisualScale;
    private Vector3 _visualScaleAtCleanStart;

    protected override void Start()
    {
        base.Start();
        _initialVisualScale = _visualTransform.localScale;
    }

    private void Update()
    {
        if (Done) return;

        if (!_isCleaning)
        {
            if (!canGrow) return;

            _growTimer += Time.deltaTime;
            float growProgress = Mathf.Clamp01(_growTimer / growDuration);
            float scale = Mathf.Lerp(1f, maxScale, growProgress);
            _visualTransform.localScale = _initialVisualScale * scale;
            return;
        }

        _cleanTimer += Time.deltaTime;
        float ratio = Mathf.Clamp01(_cleanTimer / _effectiveCleanTime);
        _visualTransform.localScale = Vector3.Lerp(_visualScaleAtCleanStart, Vector3.zero, ratio);

        if (_cleanTimer >= _effectiveCleanTime) CompleteTask();
    }

    public void Register(MopProp mop)
    {
        if (Done) return;
        mop.OnStartClean += StartClean;
        mop.OnStopClean += StopClean;
        _mop = mop;
    }

    public void Unregister(MopProp mop)
    {
        if (Done) return;
        mop.OnStartClean -= StartClean;
        mop.OnStopClean -= StopClean;
        _mop = null;
    }

    public void StartClean()
    {
        _visualScaleAtCleanStart = _visualTransform.localScale;
        float sizeMultiplier = _visualTransform.localScale.x / _initialVisualScale.x;
        _effectiveCleanTime = _mop.CleanTime * sizeMultiplier;
        _cleanTimer = 0f;
        _isCleaning = true;
    }

    public void StopClean()
    {
        _isCleaning = false;
    }

    private void CompleteTask()
    {
        MopProp mop = _mop;
        PlayerController player = mop ? mop.CurrentCleaner : null;
        mop?.RemovePuddleTask(this);

        GameManager gm = GameManager.Instance;
        if (givePointsOnClean && player && gm != null && gm.Scores != null)
        {
            if (gm.Context == GameContext.Hub)
                gm.Scores.AddTotalScore(cleanReward, player.Id);
            else if (gm.Context == GameContext.Mission)
                gm.Scores.AddMissionScore(cleanReward, player.Id);
        }

        Succeed(player);
    }
}