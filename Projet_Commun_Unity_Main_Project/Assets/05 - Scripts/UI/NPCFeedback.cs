using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NPCFeedback : MonoBehaviour
{
    [SerializeField] private float effectDuration=2.5f;
    private float _timerDuration=10f;
    
    [Header("Reaction Images")]
    [SerializeField] private Image happyImage;
    [SerializeField] private Image unhappyImage;
    
    [Header("Timer")]
    [SerializeField] private Image timerImage;
    [SerializeField] private Image timerBackdropImage;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    
    DebugConveyorTask _debugConveyorTask;
    private Coroutine _currentTimerCoroutine;

    private void Awake()
    {
        _debugConveyorTask = transform.parent.GetComponent<DebugConveyorTask>();
    }

    public void StartUITimer()
    {
        if (_currentTimerCoroutine != null)
        {
            StopCoroutine(_currentTimerCoroutine);
        }

        _currentTimerCoroutine = StartCoroutine(Timer());
        
        _timerDuration = _debugConveyorTask.Timer;
    }

    public void StopUITimer()
    {
        timerBackdropImage.gameObject.SetActive(false);
        StopCoroutine(_currentTimerCoroutine);
    }
    
    public void HappyResult()
    {
        happyImage.gameObject.SetActive(true);
        StartCoroutine(ImageFade(happyImage));
    }
    public void UnhappyResult()
    {
        unhappyImage.gameObject.SetActive(true);
        StartCoroutine(ImageFade(unhappyImage));
    }
    
    private IEnumerator Timer()
    {
        timerImage.fillAmount = 1.0f;
        timerImage.color = startColor;
        timerBackdropImage.gameObject.SetActive(true);
        float elapsedTime = 0.0f;
        
        while (timerImage.fillAmount > 0)
        {
            
            float progress = Mathf.Clamp01(elapsedTime / _timerDuration);
            
            timerImage.color = Color.Lerp(startColor, endColor, progress);
            timerImage.fillAmount = 1.0f - progress;
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        timerBackdropImage.gameObject.SetActive(false);
        yield return null;
    }
    
    private IEnumerator ImageFade(Image imageToFade)
    {
        Color startcolor = imageToFade.color;
        Color endcolor = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 0);
        float t = 0.0f;
        while (imageToFade.color.a > 0)
        {
            imageToFade.color = Color.Lerp(startcolor, endcolor, t);
            if (t < 1)
            {
                t += Time.deltaTime / effectDuration;
            }
            yield return null;
        }
        imageToFade.gameObject.SetActive(false);
        imageToFade.color = startcolor;
        yield return null;
    }
}
