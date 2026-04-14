using UnityEngine;

public class Trigger2WayTask : TriggerTask
{
    [Header("Trigger 2 Way Task")]
    [SerializeField] private PropType _badPropType = PropType.None;
    [SerializeField] private int _badScore = 100;

    protected override void OnTriggerEnter(Collider other)
    {
        if (Done || !other.TryGetComponent(out Prop prop))
        {
            prop = other.GetComponentInParent<Prop>();
            if (!prop) return;
        }
        if (prop.Type == _badPropType)
        {
            Failed();
            GameManager.Instance.Scores.SubMissionScore(_badScore, prop.OwnerId);
        }
        else if (IsTypeAccepted(prop.Type))
        {
            PlayerController playerController = prop.Controller as PlayerController;
            if (playerController == null && PlayerManager.Instance != null)
                playerController = PlayerManager.Instance.Players.Find(p => p.Id == prop.OwnerId);
    
            Succeed(playerController);
            GameManager.Instance.Scores.AddMissionScore(score, prop.OwnerId);
        }
        else return;
    
        if (isDestroyed) prop.Destroy();
    }
}