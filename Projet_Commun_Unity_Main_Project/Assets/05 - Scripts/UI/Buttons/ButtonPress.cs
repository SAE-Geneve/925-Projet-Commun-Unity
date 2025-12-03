using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPress : MonoBehaviour
{
    [Header("Game Canvas")]
    [SerializeField] Canvas currentCanvas;
    [SerializeField] Canvas newCanvas;
    
    public void ChangeCanvas()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        //currentCanvas.enabled = false;
        currentCanvas.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);

        if (newCanvas != null)
        {
            newCanvas.gameObject.SetActive(true);
            newCanvas.enabled = true;
            BasicButtonSetup buttonSetup = newCanvas.transform.GetComponent<BasicButtonSetup>();
            if (buttonSetup != null)
            {
                buttonSetup.ButtonChange();
            }
        }
        else
        {
            GameManager.Instance.PauseTrigger();
        }
    }
}
