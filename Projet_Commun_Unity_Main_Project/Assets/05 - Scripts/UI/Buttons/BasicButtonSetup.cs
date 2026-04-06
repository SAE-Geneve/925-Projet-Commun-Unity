using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BasicButtonSetup : MonoBehaviour
{
    [SerializeField] private GameObject firstButton;
    [SerializeField] private GameObject changeButton;
    private GameObject _lastSelected = null;
    private EventSystem _eventSystem;
    
    void OnEnable()
    {
        if(firstButton != null)
        {
            _eventSystem= EventSystem.current;
            _eventSystem.SetSelectedGameObject(firstButton);
        }
    }

    void Update()
    {
        if(!_eventSystem) return;
        
        if (_eventSystem.currentSelectedGameObject)
        {
            _lastSelected = _eventSystem.currentSelectedGameObject;
        }
        else
        { 
            _eventSystem.SetSelectedGameObject(_lastSelected);
        }
    }
    
    public void ButtonChange()
    {
        _eventSystem.SetSelectedGameObject(firstButton);
    }

    public void ActivateButton()
    {
        _eventSystem.SetSelectedGameObject(firstButton);
    }
}

