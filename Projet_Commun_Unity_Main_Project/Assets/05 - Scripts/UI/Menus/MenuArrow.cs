using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuArrow : MonoBehaviour
{
    [SerializeField] private List<GameObject> menuArrows;
    private GameObject _activeMenuArrow;
    
    void Update()
    {
        foreach (GameObject menuArrow in menuArrows)
        {
            if (EventSystem.current.currentSelectedGameObject == menuArrow.gameObject.transform.parent.gameObject)
            {
                _activeMenuArrow=menuArrow;
                menuArrow.SetActive(true);
            }
            else
            {
                menuArrow.SetActive(false);
            }
        }

        if (_activeMenuArrow==null)
        {
            _activeMenuArrow=menuArrows[0];
            EventSystem.current.SetSelectedGameObject(_activeMenuArrow.gameObject.transform.parent.gameObject);
            _activeMenuArrow.SetActive(true);
        }
    }
}
