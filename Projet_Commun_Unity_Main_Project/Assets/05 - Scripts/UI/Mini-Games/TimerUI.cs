using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private float Minute;
    private float Second;

    private float givenTime;
    
    [SerializeField] private TextMeshProUGUI _timerText;

    private float _timer;
    private bool _timeIncrease;
    
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
        
        if (_timer <= 0)
        {
            Second--;
            if (Second <= 0)
            {
                
                if (Minute <= 0 && Second <= 0)
                {
                    Debug.Log("Game ended");
                    //End scene
                }
                Minute--;
                Second = 59;
                
            }
            _timer=1;
        }
        _timerText.text = $"{Minute:00}:{Second:00}";
    }
}
