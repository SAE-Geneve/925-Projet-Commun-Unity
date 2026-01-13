using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.UI;

public class NPCFeedback : MonoBehaviour
{
    [SerializeField] private float effectDuration = 2.5f;
    private float _timerDuration = 20f;
    
    [Header("Reaction Images")]
    [SerializeField] private Image happyImage;
    [SerializeField] private Image unhappyImage;
    
    [Header("Timer")]
    [SerializeField] private Image timerImage;
    [SerializeField] private Image timerBackdropImage;
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    
    private Coroutine _currentTimerCoroutine;
    private AudioManager _audioManager;
    private UIFeedback _uiFeedback;
    private BehaviorGraphAgent _agent;
    
    private bool _hasTimerStarted; 

    private void Start()
    {
        if (TryGetComponent(out _uiFeedback))
        {
            Debug.Log("Found UI Text Effects");
            _uiFeedback.ImagePoolCreation(happyImage);
        }
        
        if (transform.parent.TryGetComponent(out _agent))
        {
            if (_agent.GetVariable("WaitingTime", out BlackboardVariable<float> variableContainer))
            {
                _timerDuration = variableContainer.Value;
                Debug.Log($"Collected time : {_timerDuration}");
            }
            else
            {
                Debug.LogWarning("Unable to find agent variable 'WaitingTime'");
            }
        }
        else
        {
            Debug.LogError("NPCFeedback: Impossible de trouver le BehaviorGraphAgent sur le parent !");
        }

        _audioManager = AudioManager.Instance;
    }

    private void Update()
    {
        if (_hasTimerStarted || _agent == null) return;
        
        if (_agent.GetVariable("TimerStarted", out BlackboardVariable<bool> isTimerStarted))
        {
            if (isTimerStarted.Value)
            {
                _hasTimerStarted = true;
                StartUITimer();
            }
        }
    }
    
    public void StartUITimer()
    {
        if(timerBackdropImage != null) timerBackdropImage.gameObject.SetActive(true);

        if (_currentTimerCoroutine != null)
        {
            StopCoroutine(_currentTimerCoroutine);
        }

        _currentTimerCoroutine = StartCoroutine(Timer());
    }

    public void StopUITimer()
    {
        if(timerBackdropImage != null) timerBackdropImage.gameObject.SetActive(false);
        
        if (_currentTimerCoroutine != null) 
            StopCoroutine(_currentTimerCoroutine);
    }
    
    public void HappyResult()
    {
        _audioManager.PlaySfx(_audioManager.successSFX);
        StartCoroutine(_uiFeedback.ImageFade(happyImage, effectDuration));
        StopUITimer();
    }

    public void UnhappyResult()
    {
        _audioManager.PlaySfx(_audioManager.failureSFX);
        StartCoroutine(_uiFeedback.ImageFade(unhappyImage, effectDuration));
        StopUITimer();
    }
    
    private IEnumerator Timer()
    {
        timerImage.fillAmount = 1.0f;
        timerImage.color = startColor;
        
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
    
    // private IEnumerator ImageFade(Image imageToFade)
    // {
    //     Color startcolor = imageToFade.color;
    //     Color endcolor = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 0);
    //     float t = 0.0f;
    //     while (imageToFade.color.a > 0)
    //     {
    //         imageToFade.color = Color.Lerp(startcolor, endcolor, t);
    //         if (t < 1)
    //         {
    //             t += Time.deltaTime / effectDuration;
    //         }
    //         yield return null;
    //     }
    //     imageToFade.gameObject.SetActive(false);
    //     imageToFade.color = startcolor;
    //     yield return null;
    // }
}