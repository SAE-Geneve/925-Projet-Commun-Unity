using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasicButtonSetup : MonoBehaviour
{
    [SerializeField] private GameObject _firstButton;

    public void ButtonChange()
    {
        EventSystem.current.SetSelectedGameObject(_firstButton);
    }
}

