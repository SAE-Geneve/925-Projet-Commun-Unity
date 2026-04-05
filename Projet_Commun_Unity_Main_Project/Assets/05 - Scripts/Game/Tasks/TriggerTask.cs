using System.Collections.Generic;
using UnityEngine;

public class TriggerTask : GameTask
{
    [Header("Trigger Parameters")]
    [Tooltip("The prop type that needs to touch the task collider to succeed")]
    [SerializeField] protected PropType _propType = PropType.None;
    [SerializeField] protected List<PropType> _acceptedTypes = new();
    [SerializeField] protected bool isDestroyed = true;
    [SerializeField] protected int score = 100;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (Done || !other.TryGetComponent(out Prop prop)) return;
        if (!IsTypeAccepted(prop.Type)) return;
        OnTriggerValid(prop);
    }

    private bool IsTypeAccepted(PropType type)
    {
        if (_acceptedTypes.Count > 0) return _acceptedTypes.Contains(type);
        return type == _propType;
    }

    public void SetPropType(PropType propType) => _propType = propType;

    protected virtual void OnTriggerValid(Prop prop)
    {
        PlayerController playerController = prop.Controller as PlayerController;
        if (playerController == null && PlayerManager.Instance != null)
            playerController = PlayerManager.Instance.Players.Find(p => p.Id == prop.OwnerId);
        Succeed(playerController);

        GameManager gm = GameManager.Instance;

        if (gm)
        {
            if (gm.CurrentMission != null) gm.Scores.AddMissionScore(score, prop.OwnerId);
            else gm.Scores.AddPlayerScore(score, prop.OwnerId);
        }
        if (isDestroyed) prop.Destroy();
    }
}