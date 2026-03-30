using UnityEngine;

public class ConveyorBeltTask : TriggerTask
{
    [SerializeField] private ParticleSystem electricEffect;
    
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
            ScoreSystem.IncreaseScore(1);
        }
        else
        {
            Failed();
            ScoreSystem.IncreaseScore(3);
        }

        if(_conveyorBreakdown != null && _conveyorBreakdown.IsEventActive())
            electricEffect?.Play();

        prop.Grabbed(_controller);
        _controller.SetGrabbedProp(prop);
    }
}