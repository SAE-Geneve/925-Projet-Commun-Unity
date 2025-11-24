using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    private float _minute;
    private float _second;

    private float _givenTime;

    [SerializeField] private TextMeshProUGUI timerText;
    
    [SerializeField] private TextMeshProUGUI timeRemainingText;
    [SerializeField] private Image timeRemainingImage;
    private int _reminder;

    private float _timer;
    private UIScreenEffects _uiScreenEffects;

    void Start()
    {
        //Intiliaze the timer
        _givenTime = GameManager.Instance.CurrentMission.Timer;

        _minute = _givenTime / 60;
        _second = _givenTime % 60;

        timerText.text = $"{_minute:00}:{_second:00}";
        
        _uiScreenEffects=transform.parent.parent.GetComponent<UIScreenEffects>();
    }

    void FixedUpdate()
    {
        if (_givenTime > 0 && GameManager.Instance.State != GameState.Paused)
        {
            _givenTime -= Time.deltaTime;

            // Divide the time by 60
            _minute = Mathf.FloorToInt(_givenTime / 60);

            // Returns the remainder
            _second = Mathf.FloorToInt(_givenTime % 60);

            //Set text string
            timerText.text = $"{_minute:00}:{_second:00}";
        }

        if (_minute == 0 && _second == 30f && _reminder == 0)
        {
            TimeRemaining(30);
            _reminder++;
        }
        else if (_minute == 0 && _second == 15f && _reminder == 1)
        {
            TimeRemaining(15);
            _reminder++;
        }
    }

    void TimeRemaining(int time)
    {
        timeRemainingText.text = $"{time} seconds remaining.";
        StartCoroutine(_uiScreenEffects.DoTextFade(timeRemainingText));
        StartCoroutine(_uiScreenEffects.DoImageFade(timeRemainingImage));
    }
}