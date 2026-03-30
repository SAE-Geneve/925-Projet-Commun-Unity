using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private GameObject _root;

    private void Awake()
    {
        _root.SetActive(false);
        Mission.OnCountdownTick += OnTick;
        Mission.OnCountdownEnd += OnEnd;
    }

    private void OnDestroy()
    {
        Mission.OnCountdownTick -= OnTick;
        Mission.OnCountdownEnd -= OnEnd;
    }

    private void OnTick(int remaining)
    {
        _root.SetActive(true);
        _countdownText.SetText(remaining.ToString());
    }

    private void OnEnd()
    {
        _root.SetActive(false);
    }
}
