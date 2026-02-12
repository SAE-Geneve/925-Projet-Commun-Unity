using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PernamentTimerUI : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Image backdropScore;
    [SerializeField] private GameObject scoreObject;
    [SerializeField] private GameObject timerObject;

    [Header("Colors")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;

    [Header("Positiions")]
    [SerializeField] private Transform timerInMinigame;
    [SerializeField] private Transform timerOutMinigame;

    private float _totalTime;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _totalTime = _gameManager.Timer;
    }

    private void Update()
    {
        int minutes = Mathf.FloorToInt(_gameManager.Timer / 60f);
        int seconds = Mathf.FloorToInt(_gameManager.Timer % 60f);

        Debug.Log(minutes + " and " + seconds);
        timeText.SetText($"{minutes:00}:{seconds:00}");

        float t = 1f - Mathf.Clamp01(_gameManager.Timer / _totalTime);
        backdropScore.color = Color.Lerp(startColor, endColor, t);
    }

    public void InMinigameVisual()
    {
        scoreObject.SetActive(false);
        StartCoroutine(MoveToPosition(timerInMinigame));
    }

    public void OutMinigameVisual()
    {
        moneyText.text = ScoreSystem.TotalMoneyScore + "$";
        StartCoroutine(MoveToPosition(timerOutMinigame));
    }

    private IEnumerator MoveToPosition(Transform newPosition)
    {
        while (Vector3.Distance(timerObject.transform.position, newPosition.position) > 0.1f)
        {
            timerObject.gameObject.transform.position = Vector3.MoveTowards(timerObject.gameObject.transform.position,
                newPosition.position, Time.deltaTime*100);
            yield return null;
        }

        if (newPosition.position == timerOutMinigame.position)
        {
            scoreObject.SetActive(true);
        }
    }
}