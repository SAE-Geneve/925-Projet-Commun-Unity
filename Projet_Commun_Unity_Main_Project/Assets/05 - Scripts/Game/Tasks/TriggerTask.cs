using UnityEngine;

public class TriggerTask : GameTask
{
    [Header("Trigger Parameters")]
    [Tooltip("The prop type that needs to touch the task collider to succeed")]
    [SerializeField] protected PropType _propType = PropType.None;
    [SerializeField] protected bool isDestroyed = true;
    [SerializeField] protected int score = 100;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(Done || !other.TryGetComponent(out Prop prop) || prop.Type != _propType) return;
        
        Succeed();
        
        GameManager gm = GameManager.Instance;
        
        if (gm)
        {
            if (gm.CurrentMission != null) gm.Scores.AddMissionScore(score, prop.OwnerId);
            else gm.Scores.AddTotalScore(score, prop.OwnerId);
        }
        
        if(isDestroyed) prop.Destroy();
    }
    
    public void SetPropType(PropType propType) => _propType = propType;
}