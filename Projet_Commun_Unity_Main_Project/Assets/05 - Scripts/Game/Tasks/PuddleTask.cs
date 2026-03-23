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

    private MopProp _mop;
    private Material _puddleMaterial;

    private float _cleanTimer;
    private bool _isCleaning;

    private float _growTimer = 0f;
    private Vector3 _initialScale;

    protected override void Start()
    {
        base.Start();
        _puddleMaterial = GetComponent<Renderer>().material;
        _initialScale = transform.localScale;
    }

    private void Update()
    {
        if (Done) return;
        if (canGrow)
        {
            _growTimer += Time.deltaTime;
            float growProgress = Mathf.Clamp01(_growTimer / growDuration);
            transform.localScale = _initialScale * Mathf.Lerp(1f, maxScale, growProgress);
        }
        if (!_isCleaning) return;

        float cleanTime = _mop.CleanTime;
        float ratio = Mathf.Clamp01(_cleanTimer / cleanTime);
        UpdateMaterial(ratio);

        _cleanTimer += Time.deltaTime;

        if (_cleanTimer >= cleanTime) CompleteTask();
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

    public void StartClean() => _isCleaning = true;
    public void StopClean() => _isCleaning = false;

    private void CompleteTask()
    {
        PlayerController player = _mop != null ? _mop.CurrentCleaner : null;
        _mop.RemovePuddleTask(this);

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

    private void UpdateMaterial(float ratio)
    {
        Color color = _puddleMaterial.color;
        color.a = 1f - ratio;
        _puddleMaterial.color = color;
    }
}