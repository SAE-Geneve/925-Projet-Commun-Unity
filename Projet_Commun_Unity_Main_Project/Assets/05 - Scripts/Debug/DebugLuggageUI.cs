using TMPro;
using UnityEngine;

public class DebugLuggageUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _clientSatisfiedTmp;
    [SerializeField] private TextMeshProUGUI _clientUnsatisfiedTmp;

    private int _clientSatisfied;
    private int _clientUnsatisfied;
    
    private void Start()
    {
        _clientSatisfiedTmp.SetText($"Satisfied Clients : {_clientSatisfied}");
        _clientUnsatisfiedTmp.SetText($"Unsatisfied Clients : {_clientUnsatisfied}");
    }

    public void AddSatisfiedClient()
    {
        _clientSatisfiedTmp.SetText($"Satisfied Clients : {++_clientSatisfied}");
    }

    public void AddUnsatisfiedClient()
    {
        _clientUnsatisfiedTmp.SetText($"Unsatisfied Clients : {++_clientUnsatisfied}");
    }
}
