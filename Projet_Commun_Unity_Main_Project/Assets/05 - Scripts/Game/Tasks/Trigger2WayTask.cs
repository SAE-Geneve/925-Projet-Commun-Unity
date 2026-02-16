using UnityEngine;

public class Trigger2WayTask : TriggerTask
{
    [Header("Trigger 2 Way Task")]
    [SerializeField] private PropType _badPropType = PropType.None;
    [SerializeField] private int _badScore = 100;

    protected override void OnTriggerEnter(Collider other)
    {
        if(Done || !other.TryGetComponent(out Prop prop)) return;

        if (prop.Type == _propType)
        {
            Succeed();
            GameManager.Instance.Scores.AddMissionScore(score, prop.OwnerId);
        }
        else if (prop.Type == _badPropType)
        {
            Failed();
            GameManager.Instance.Scores.DecreaseMissionScore(_badScore, prop.OwnerId);
        }
        else return;
        


        if (isDestroyed) prop.Destroy();
    }
}
