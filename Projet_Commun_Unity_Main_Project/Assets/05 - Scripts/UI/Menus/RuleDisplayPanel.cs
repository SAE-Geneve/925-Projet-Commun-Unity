using System.Collections;
using TMPro;
using UnityEngine;

public class RuleDisplayPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    private float _waitTime = 10f;
    

    // Update is called once per frame
    void OnEnable()
    {
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        while (_waitTime > 0f)
        {
            _waitTime -= 1;
            int seconds = Mathf.FloorToInt(_waitTime % 60f);
            timeText.SetText($"{seconds:00}");
            yield return new WaitForSecondsRealtime(1f);
        }
        
        yield return null;
    }
}
