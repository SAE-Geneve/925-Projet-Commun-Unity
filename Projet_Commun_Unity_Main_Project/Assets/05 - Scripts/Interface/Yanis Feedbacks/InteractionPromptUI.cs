using System.Collections;
using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI promptText;

    private void Awake()
    {
        root.SetActive(false);
    }

    public void Show(string text)
    {
        root.SetActive(true);
        promptText.text = text;
    }

    public void Hide()
    {
        root.SetActive(false);
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        Vector3 originalScale = root.transform.localScale;

        root.transform.localScale = originalScale * 1.15f;
        yield return new WaitForSeconds(0.05f);
        root.transform.localScale = originalScale;
    }
}