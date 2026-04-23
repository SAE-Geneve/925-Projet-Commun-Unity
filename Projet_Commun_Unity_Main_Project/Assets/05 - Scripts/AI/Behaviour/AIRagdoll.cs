using UnityEngine;
using Unity.Behavior;

public class AIRagdoll : Ragdoll
{
    [SerializeField] private BehaviorGraphAgent behaviorGraphAgent;
    [SerializeField] private GameObject bHips;
    [SerializeField] private bool isRagdollable = true;
    [SerializeField] private ParticleSystem hitEffect;
    [Header("Drop Settings")]
    [Tooltip("Si coché, l'IA lâche ce qu'elle tient quand elle tombe en Ragdoll")]
    [SerializeField] private bool dropObjectOnRagdoll = true; 

    [Header("Score Penalty")]
    [SerializeField] private bool losePointsOnRagdoll = true;
    [SerializeField] private int ragdollPenalty = 50;
    public bool IsRagdollState { get; private set; }
    
    private float _lastRagdollOffTime;
    private Rigidbody _mainRb;
    
    public PlayerController Catcher { get; private set; }
    public bool CaughtByPlayer => Catcher != null;

    protected override void Start()
    {
        base.Start();
        _mainRb = GetComponent<Rigidbody>();
    }
    
    protected override void OnRagdolledBy(GameObject striker)
    {
        PlayerController playerController = striker.GetComponentInParent<PlayerController>();
        if (playerController == null && striker.CompareTag("Player"))
            playerController = striker.GetComponent<PlayerController>();
        if (playerController == null)
            playerController = PlayerManager.Instance?.Players.Find(
                p => p.Rb != null && Vector3.Distance(p.transform.position, transform.position) < 3f
            );
        if (playerController != null)
        {
            Catcher = playerController;
            hitEffect?.Play();
        
            if (!losePointsOnRagdoll) return;
        
            if (GameManager.Instance != null && GameManager.Instance.Scores != null)
            {
                if (GameManager.Instance.Context == GameContext.Hub)
                    GameManager.Instance.Scores.SubPlayerScore(ragdollPenalty, playerController.Id);
                else
                    GameManager.Instance.Scores.SubPlayerScore(ragdollPenalty, playerController.Id);
            }
        }
    }
    public override void RagdollOn(bool ignoreImmunity = false)
    {
        if (Time.time < _lastRagdollOffTime + 1.0f) return;
        if (!isRagdollable || IsRagdollState) return;

        // --- LOGIQUE DE DROP ---
        if (dropObjectOnRagdoll)
        {
            Controller ctrl = GetComponent<Controller>();
            if (ctrl != null)
            {
                ctrl.Drop();
            }
        }

        Vector3 currentVelocity = Vector3.zero;
        if (_mainRb != null) currentVelocity = _mainRb.linearVelocity;

        IsRagdollState = true;
        bHips.SetActive(true);

        Collider[] cols = bHips.GetComponentsInChildren<Collider>(true);
        foreach (var col in cols) col.enabled = true;

        Rigidbody[] rbs = bHips.GetComponentsInChildren<Rigidbody>(true);
        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
            rb.linearVelocity = currentVelocity;
        }

        base.RagdollOn();
        SetVariableInBlackboard(true, "IsRagdoll");
    }

    public void ClearCatcher() => Catcher = null;

    protected override void RagdollOff()
    {
        if (!IsRagdollState) return;

        Rigidbody[] rbs = bHips.GetComponentsInChildren<Rigidbody>(true);
        foreach (var rb in rbs)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        Collider[] cols = bHips.GetComponentsInChildren<Collider>(true);
        foreach (var col in cols) col.enabled = false;

        bHips.SetActive(false);
        IsRagdollState = false;
        _lastRagdollOffTime = Time.time;

        base.RagdollOff();

        if (_mainRb)
        {
            _mainRb.linearVelocity = Vector3.zero;
            _mainRb.angularVelocity = Vector3.zero;
        }

        SetVariableInBlackboard(false, "IsRagdoll");
        ClearCatcher();
    }

    private void SetVariableInBlackboard<T>(T value, string variableName)
    {
        if (!behaviorGraphAgent) return;
        if (behaviorGraphAgent.GetVariable<T>(variableName, out var variable))
            variable.Value = value;
    }
}