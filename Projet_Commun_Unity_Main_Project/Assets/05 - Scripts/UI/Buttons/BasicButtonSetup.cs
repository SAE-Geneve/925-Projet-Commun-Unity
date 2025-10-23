using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

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
        if (EventSystem.current.currentSelectedGameObject != null)
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

