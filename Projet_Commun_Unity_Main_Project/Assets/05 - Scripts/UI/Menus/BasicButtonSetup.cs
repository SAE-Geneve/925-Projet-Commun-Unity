using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasicButtonSetup : MonoBehaviour
{
    [SerializeField] private GameObject _firstButton;

    //Horrid one time script, will be replaced by a better one
    public void ButtonChange()
    {
        EventSystem.current.SetSelectedGameObject(_firstButton);
    }
}

