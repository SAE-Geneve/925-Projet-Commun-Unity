using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    
    [SerializeField] private TextMeshProUGUI timeRemainingText;
    [SerializeField] private Image timeRemainingImage;
    
    private UIScreenEffects _uiScreenEffects;
    
    private Mission _mission;
    
    private float _minute;
    private float _second;
    
    private int _reminder;

    void OnEnable()
    {
        //Intiliaze the timer
        _mission = GameManager.Instance.CurrentMission;
        
        _uiScreenEffects = transform.parent.parent.GetComponent<UIScreenEffects>();
        
        UpdateTimer();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.Context == GameContext.Mission)
        {
            UpdateTimer();

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
    }

    private void TimeRemaining(int time)
    {
        if (timeRemainingText != null)
        {
            timeRemainingText.text = $"{time} seconds remaining.";
            StartCoroutine(_uiScreenEffects.DoTextFade(timeRemainingText));
            StartCoroutine(_uiScreenEffects.DoImageFade(timeRemainingImage));
        }
    }

    private void UpdateTimer()
    {
        float timer = _mission.Timer;

        _minute = Mathf.FloorToInt(timer / 60);
        _second = Mathf.FloorToInt(timer % 60);
        
        timerText.text = $"{_minute:00}:{_second:00}";
    }
}