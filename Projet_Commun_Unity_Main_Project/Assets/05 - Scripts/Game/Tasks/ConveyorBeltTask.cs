using UnityEngine;

public class ConveyorBeltTask : TriggerTask
{
    [SerializeField] private ParticleSystem electricEffect;
    
    [Header("Score Settings")]
    [SerializeField] private int correctScore = 1;
    [SerializeField] private int wrongScore = 3;
    
    private Controller _controller;
    private ConveyorBreakdownEvent _conveyorBreakdown;

    protected override void Start()
    {
        base.Start();
        _controller = GetComponent<Controller>();
    }

    public void SetBreakdownEvent(ConveyorBreakdownEvent breakdown)
    {
        _conveyorBreakdown = breakdown;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if(Done || !other.TryGetComponent(out Prop prop)) return;
        if(prop.IsGrabbed) return;

        bool isCorrect = prop.Type == _propType;

        if(isCorrect)
        {
            Succeed();
            GameManager.Instance.Scores.AddMissionScore(score, prop.OwnerId);
            GameManager.Instance.Scores.AddMissionScore(correctScore, prop.OwnerId);
        }
        else
        {
            Failed();
            GameManager.Instance.Scores.SubMissionScore(wrongScore, prop.OwnerId);
        }

        if(_conveyorBreakdown != null && _conveyorBreakdown.IsEventActive())
            electricEffect?.Play();

        prop.Grabbed(_controller);
        _controller.SetGrabbedProp(prop);
    }
}