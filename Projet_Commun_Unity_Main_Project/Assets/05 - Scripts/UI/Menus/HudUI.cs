using TMPro;
using UnityEngine;

public class HudUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalMoneyScore;
    [SerializeField] private TextMeshProUGUI totalTimeScore;

    private GameManager _gameManager;

    // private void Start() => _gameManager = GameManager.Instance;

    public void UpdateHUDUI()
    {
        totalMoneyScore.text = ScoreSystem.TotalMoneyScore+"$";
        // totalTimeScore.text = ScoreSystem.TotalTimeCollected+"s";
    }

    // private void Update()
    // {
    //     int minutes = Mathf.FloorToInt(_gameManager.Timer / 60f);
    //     int seconds = Mathf.FloorToInt(_gameManager.Timer % 60f);
    //
    //     totalTimeScore.SetText($"{minutes:00}:{seconds:00}");
    // }

}
