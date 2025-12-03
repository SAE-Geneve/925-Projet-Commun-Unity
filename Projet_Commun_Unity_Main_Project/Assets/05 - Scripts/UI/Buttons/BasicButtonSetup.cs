using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BasicButtonSetup : MonoBehaviour
{
    [SerializeField] private GameObject firstButton;
    private GameObject _lastSelected = null;
    
    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject)
        {
            _lastSelected = EventSystem.current.currentSelectedGameObject;
        }
        else
        { 
            EventSystem.current.SetSelectedGameObject(_lastSelected);
        }
    }
    
    public void ButtonChange()
    {
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
}

