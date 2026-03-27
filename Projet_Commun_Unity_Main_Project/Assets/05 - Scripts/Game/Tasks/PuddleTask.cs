using UnityEngine;

public class PuddleTask : GameTask
{
    [Header("Score Reward (Hub Only)")]
    [SerializeField] private bool givePointsOnClean = false;
    [SerializeField] private int cleanReward = 50;

    [Header("Growth Settings")]
    [Tooltip("Coche cette case uniquement pour les flaques qui doivent grandir")]
    [SerializeField] private bool canGrow = false;
    [Tooltip("Taille max (multiplicateur de la scale initiale)")]
    [SerializeField] private float maxScale = 3f;
    [Tooltip("Durée en secondes pour atteindre la taille max")]
    [SerializeField] private float growDuration = 15f;

    [Header("Puddle References")]
    [Tooltip("Child object that holds the meshes and renderer")]
    [SerializeField] private Transform _visualTransform;

    private MopProp _mop;
    private SphereCollider _ragdollCollider;

    private float _cleanTimer;
    private float _effectiveCleanTime;
    private bool _isCleaning;

    private float _growTimer = 0f;
    private float _initialRadius;
    private float _radiusAtCleanStart;
    private Vector3 _initialVisualScale;
    private Vector3 _visualScaleAtCleanStart;

    protected override void Start()
    {
        base.Start();
        _ragdollCollider = GetComponent<SphereCollider>();
        _initialRadius = _ragdollCollider.radius;
        _initialVisualScale = _visualTransform.localScale;
    }

    private void Update()
    {
        if (Done) return;

        if (canGrow && !_isCleaning)
        {
            _growTimer += Time.deltaTime;
            float growProgress = Mathf.Clamp01(_growTimer / growDuration);
            float scale = Mathf.Lerp(1f, maxScale, growProgress);
            _ragdollCollider.radius = _initialRadius * scale;
            _visualTransform.localScale = _initialVisualScale * scale;
        }

        if (!_isCleaning) return;

        _cleanTimer += Time.deltaTime;
        float ratio = Mathf.Clamp01(_cleanTimer / _effectiveCleanTime);
        _ragdollCollider.radius = Mathf.Lerp(_radiusAtCleanStart, 0f, ratio);
        _visualTransform.localScale = Vector3.Lerp(_visualScaleAtCleanStart, Vector3.zero, ratio);

        if (_cleanTimer >= _effectiveCleanTime) CompleteTask();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Ragdoll ragdoll)) ragdoll.RagdollOn();
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
        _radiusAtCleanStart = _ragdollCollider.radius;
        _visualScaleAtCleanStart = _visualTransform.localScale;
        float sizeMultiplier = _radiusAtCleanStart / _initialRadius;
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
        PlayerController player = mop != null ? mop.CurrentCleaner : null;
        mop?.RemovePuddleTask(this);

        if (givePointsOnClean && player != null)
        {
            if (GameManager.Instance != null && GameManager.Instance.Scores != null)
            {
                if (GameManager.Instance.Context == GameContext.Hub)
                    GameManager.Instance.Scores.AddTotalScore(cleanReward, player.Id);
                else if (GameManager.Instance.Context == GameContext.Mission)
                    GameManager.Instance.Scores.AddMissionScore(cleanReward, player.Id);
            }
        }
        Succeed(player);
    }
}