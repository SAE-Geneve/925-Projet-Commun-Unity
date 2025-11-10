using UnityEngine;

public class PuddleTask : GameTask
{
    [Header("Parameters")] 
    [SerializeField] private float _cleanTime = 2f;

    private MopProp _mop;
    private Material _puddleMaterial;

    private float _cleanTimer;
    private bool _isCleaning;

    protected override void Start()
    {
        base.Start();
        _puddleMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if(!_isCleaning) return;
        
        float ratio = Mathf.Clamp01(_cleanTimer / _cleanTime);
        UpdateMaterial(ratio);
        
        _cleanTimer += Time.deltaTime;

        if (_cleanTimer >= _cleanTime) CompleteTask();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Ragdoll ragdoll))
            ragdoll.RagdollOn();
    }

    public void Register(MopProp mop)
    {
        if(Done) return;
        mop.OnStartClean += StartClean;
        mop.OnStopClean += StopClean;
        _mop = mop;
    }

    public void Unregister(MopProp mop)
    {
        if(Done) return;
        mop.OnStartClean -= StartClean;
        mop.OnStopClean -= StopClean;
        _mop = null;
    }

    public void StartClean() => _isCleaning = true;
    public void StopClean() => _isCleaning = false;

    private void CompleteTask()
    {
        _mop.RemovePuddleTask(this);
        
        Succeed();
    }
    
    private void UpdateMaterial(float ratio)
    {
        Color color = _puddleMaterial.color;
        color.a = 1f - ratio;
        _puddleMaterial.color = color;
    }
}
