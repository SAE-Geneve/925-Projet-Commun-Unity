using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private float Minute;
    private float Second;

    private float givenTime;
    
    [SerializeField] private TextMeshProUGUI _timerText;

    private float _timer;
    private bool _stopTimer;
    
    void Start()
    {
        //Intiliaze the timer
        givenTime = GameManager.Instance.CurrentMission.Timer;
        Minute = givenTime / 60;
        Second = givenTime % 60;
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        
        if (_timer <= 0 && !_stopTimer)
        {
            Second--;
            if (Second <= 0)
            {
                
                if (Minute <= 0 && Second <= 0)
                {
                    _stopTimer = true;
                    _timerText.text = "00:00";
                    GameManager.Instance.CurrentMission.Finish();
                }
                else
                {
                    Minute--;
                    Second = 59;
                }
                
            }
            _timer=1;
        }

        if (!_stopTimer)
        {
            _timerText.text = $"{Minute:00}:{Second:00}";
        }
    }
}
