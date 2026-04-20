using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private SphereCollider _mopZoneCollider;

    [Header("UI")]
    [SerializeField] private Image donutImage;

    private MopProp _mop;

    private float _cleanTimer;
    private float _effectiveCleanTime;
    private bool _isCleaning;

    private float _growTimer = 0f;
    private Vector3 _initialVisualScale;
    private Vector3 _visualScaleAtCleanStart;
    private float _initialMopZoneRadius;

    protected override void Start()
    {
        base.Start();
        _initialVisualScale = _visualTransform.localScale;
        if (_mopZoneCollider) _initialMopZoneRadius = _mopZoneCollider.radius;
        
        if (donutImage != null)
        {
            donutImage.fillAmount = 0f;
            donutImage.gameObject.SetActive(false);
        }
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
            
            if (_mopZoneCollider)
                _mopZoneCollider.radius = _initialMopZoneRadius * scale;
        
            return;
        }

        _cleanTimer += Time.deltaTime;
        float ratio = Mathf.Clamp01(_cleanTimer / _effectiveCleanTime);
        if (donutImage != null) 
        {
            donutImage.fillAmount = ratio;
        }

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
        if (!_visualTransform) return;

        _visualScaleAtCleanStart = _visualTransform.localScale;
        float sizeMultiplier = _visualTransform.localScale.x / _initialVisualScale.x;
        _effectiveCleanTime = _mop.CleanTime * sizeMultiplier;
        _cleanTimer = 0f;
        _isCleaning = true;
        canGrow = false;
        
        if (donutImage != null)
        {
            donutImage.gameObject.SetActive(true);
        }
    }

    public void StopClean()
    {
        _isCleaning = false;

        if (_mopZoneCollider && _visualTransform)
        {
            float currentScale = _visualTransform.localScale.x / _initialVisualScale.x;
            _mopZoneCollider.radius = _initialMopZoneRadius * currentScale;
        }
        
        if (donutImage != null)
        {
            donutImage.gameObject.SetActive(false);
        }
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
            {
                gm.Scores.AddPlayerScore(cleanReward, player.Id);
                
                // On déclenche le feedback visuel directement sur le joueur
                CharacterDisplay display = player.GetComponentInChildren<CharacterDisplay>();
                if (display != null) display.ShowScoreFeedback(cleanReward);
            }
            else if (gm.Context == GameContext.Mission)
            {
                gm.Scores.AddMissionScore(cleanReward, player.Id);
            }
        }
        
        if (donutImage != null)
        {
            donutImage.gameObject.SetActive(false);
        }

        Succeed(player);
    }
}