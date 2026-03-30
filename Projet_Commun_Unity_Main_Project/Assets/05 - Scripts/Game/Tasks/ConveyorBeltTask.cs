using UnityEngine;

public class ConveyorBeltTask : TriggerTask
{
    private Controller _controller;
    protected override void Start()
    {
        base.Start();
        
        _controller = GetComponent<Controller>();
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

        prop.Grabbed(_controller);
        _controller.SetGrabbedProp(prop);
    }
}
