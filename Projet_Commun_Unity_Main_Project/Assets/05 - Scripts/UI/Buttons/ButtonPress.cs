using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//[ExecuteInEditMode]
public class ButtonPress : MonoBehaviour
{
    //Shouldn't be universal (Like main menu does not care about in-game canvas)
    [Header("Game Canvas")]
    [SerializeField] Canvas currentCanvas;
    [SerializeField] Canvas newCanvas;
    
    public void ChangeCanvas()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        currentCanvas.enabled = false;
        currentCanvas.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        
        newCanvas.gameObject.SetActive(true);
        newCanvas.enabled = true;
        BasicButtonSetup buttonSetup = newCanvas.transform.GetComponent<BasicButtonSetup>();
        if (buttonSetup != null)
        {
            buttonSetup.ButtonChange();
        }
    }
}
