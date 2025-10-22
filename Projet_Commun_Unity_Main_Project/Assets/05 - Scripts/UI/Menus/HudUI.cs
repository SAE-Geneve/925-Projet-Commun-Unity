using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class HudUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalMoneyScore;
    private int _moneyScore;
    [SerializeField] private TextMeshProUGUI totalTimeScore;
    private int _timeScore;
    
    public void SetMoneyScore(int score)
    {
        _moneyScore += score;
        totalMoneyScore.text = _moneyScore.ToString()+"$";
    }
    
    public void SetTimeScore(int score)
    {
        _timeScore += score;
        totalTimeScore.text = _timeScore.ToString()+"s";
    }
}
