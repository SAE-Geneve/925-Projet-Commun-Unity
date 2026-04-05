using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPress : MonoBehaviour
{
    [Header("References")] [SerializeField]
    private Animator animator;

    [Header("Game Canvas")] [SerializeField]
    Canvas currentCanvas;

    [SerializeField] Canvas newCanvas;

    public void ChangeCanvas()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        if (animator && newCanvas && currentCanvas)
        {
            StartCoroutine(LoadCanvasAfterAnimation());
            return;
        }

        //currentCanvas.enabled = false;
        if (currentCanvas != null)
        {
            currentCanvas.gameObject.SetActive(false);
        }

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

    private IEnumerator LoadCanvasAfterAnimation()
    {
        animator.gameObject.SetActive(true);

        animator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);

        currentCanvas.gameObject.SetActive(false);
        newCanvas.gameObject.SetActive(true);

        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
        animator.gameObject.SetActive(false);
    }
}