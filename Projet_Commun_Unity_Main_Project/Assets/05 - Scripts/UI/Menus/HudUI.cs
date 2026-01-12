using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class HudUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalMoneyScore;
    [SerializeField] private TextMeshProUGUI totalTimeScore;

    public void UpdateHUDUI()
    {
        totalMoneyScore.text = ScoreSystem.TotalMoneyScore+"$";
        totalTimeScore.text = ScoreSystem.TotalTimeCollected+"s";
    }
}
