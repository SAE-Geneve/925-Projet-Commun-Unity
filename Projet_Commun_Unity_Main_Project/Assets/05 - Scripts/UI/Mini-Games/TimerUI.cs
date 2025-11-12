using TMPro;
using UnityEngine;
using System.Collections;

public class TimerUI : MonoBehaviour
{
    private float Minute;
    private float Second;

    private float givenTime;

    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _timeRemainingText;

    private float _timer;
    private UIScreenEffects _uiScreenEffects;
    // private bool _stopTimer;

    void Start()
    {
        //Intiliaze the timer
        givenTime = GameManager.Instance.CurrentMission.Timer;

        Minute = givenTime / 60;
        Second = givenTime % 60;

        _timerText.text = $"{Minute:00}:{Second:00}";
        
        _uiScreenEffects=transform.parent.GetComponent<UIScreenEffects>();
    }

    void FixedUpdate()
    {
        if (givenTime > 0 && GameManager.Instance.State != GameState.Paused)
        {
            givenTime -= Time.deltaTime;

            // Divide the time by 60
            Minute = Mathf.FloorToInt(givenTime / 60);

            // Returns the remainder
            Second = Mathf.FloorToInt(givenTime % 60);

            //Set text string
            _timerText.text = $"{Minute:00}:{Second:00}";
        }

        if (Minute == 0 && Second == 30)
        {
            TimeRemaining(30);
        }
    }

    void TimeRemaining(int time)
    {
        _timeRemainingText.text = $"{time} seconds remaining.";
        StartCoroutine(_uiScreenEffects.DoTextFade(_timeRemainingText));
    }
}