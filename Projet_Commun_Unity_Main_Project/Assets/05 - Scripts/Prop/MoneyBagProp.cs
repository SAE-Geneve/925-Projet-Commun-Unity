using UnityEngine;

public class MoneyBagProp : InteractableProp
{
    [Header("Score Settings")]
    [Tooltip("Les points donnés au joueur qui ramasse le sac")]
    [SerializeField] private int _scoreReward = 200;

    public override void Interact(PlayerController playerController)
    {
        if (playerController == null) return;
        
        if (GameManager.Instance != null && GameManager.Instance.Scores != null)
        {
            if (GameManager.Instance.Context == GameContext.Hub)
            {
                GameManager.Instance.Scores.AddTotalScore(_scoreReward, playerController.Id);
            }
            else
            {
                GameManager.Instance.Scores.AddMissionScore(_scoreReward, playerController.Id);
            }
            
            Debug.Log($"[Score] Joueur {playerController.Id} a récupéré le sac d'argent ! +{_scoreReward}");
        }
        
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        
        Destroy(gameObject);
    }
}