using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    [Header("Event Score Settings")]
    [Tooltip("Points accordés au joueur qui résout cet événement (ou une étape de cet événement)")]
    [SerializeField] protected int _eventScoreReward = 50;

    public abstract void TriggerEvent();

    public abstract void ResetEvent();

    public abstract bool IsEventActive();
    
    protected virtual void RewardPlayer(PlayerController player, int customScore = -1)
    {
        if (player != null && GameManager.Instance != null && GameManager.Instance.Scores != null)
        {
            int scoreToGive = customScore >= 0 ? customScore : _eventScoreReward;
            
            GameManager.Instance.Scores.AddPlayerScore(scoreToGive, player.Id);
            Debug.Log($"[Score] {scoreToGive} points accordés au joueur {player.Id} ({gameObject.name})");
        }
    }
}