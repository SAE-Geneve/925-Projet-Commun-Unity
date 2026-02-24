using UnityEngine;
using UnityEngine.UI;
using System;

public class MissionExplanationUI : MonoBehaviour
{
    [Header("Référence")]
    [SerializeField] private Button _continueButton;

    public event Action OnContinueClicked;

    private void Start()
    {
        if (_continueButton != null)
        {
            _continueButton.onClick.AddListener(() => 
            {
                OnContinueClicked?.Invoke();
            });
        }
        else
        {
            Debug.LogError("Attention : Le bouton Continue n'est pas assigné dans l'inspecteur du prefab !");
        }
    }
}