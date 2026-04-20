using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionPointer : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private Mission mission;
    
    [Header("Arrow Sprites")]
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;
    [SerializeField] private Sprite missionSprite;
    
    [Header("UI References")]
    [SerializeField] private Image missionIcon;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private Image pointerImage;
    [SerializeField] private RectTransform arrowSystemRect;
    
    private Camera _mainCamera;
    private Vector3 _targetPosition;
    private RectTransform _canvasTransform;
    private bool _isMissionCooldown;
    private float _missionTimer;

    private void Start()
    {
        _mainCamera = Camera.main;
        _targetPosition = mission.ArrowTarget.position;
        _canvasTransform = GetComponent<RectTransform>();
        missionIcon.sprite = missionSprite;
        cooldownText.gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 targetPositionScreenPoint = _mainCamera.WorldToScreenPoint(_targetPosition);
        
        float borderSize = 125f;
        bool isOffScreen = targetPositionScreenPoint.x < borderSize ||
                           targetPositionScreenPoint.x > Screen.width - borderSize ||
                           targetPositionScreenPoint.y < borderSize + 50 ||
                           targetPositionScreenPoint.y > Screen.height - borderSize - 50;

        if (isOffScreen)
        {
            HandleOffScreen(targetPositionScreenPoint, borderSize);
        }
        else
        {
            HandleOnScreen(targetPositionScreenPoint);
        }

        if (!_isMissionCooldown && mission.IsLocked)
        {
            HandleMissionLocked();
        }
        else if (_isMissionCooldown && !mission.IsLocked)
        {
            HandleMissionUnlocked();
        }
        
        if (_isMissionCooldown)
        {
            _missionTimer -= Time.deltaTime;
            cooldownText.text = ((int)_missionTimer).ToString();
        }
    }

    private void HandleOffScreen(Vector3 targetScreenPoint, float borderSize)
    {
        // Rotate just the pointer image
        RotatePointerTowardsTarget(targetScreenPoint);
        pointerImage.sprite = arrowSprite;
        
        // Clamp position to screen edges
        Vector3 clampedScreenPos = targetScreenPoint;
        clampedScreenPos.x = Mathf.Clamp(clampedScreenPos.x, borderSize, Screen.width - borderSize);
        clampedScreenPos.y = Mathf.Clamp(clampedScreenPos.y, borderSize, Screen.height - borderSize);
        
        // Move the entire arrow system to the clamped position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasTransform,
            clampedScreenPos,
            null,
            out Vector2 localPos
        );
        arrowSystemRect.anchoredPosition = localPos;
    }

    private void HandleOnScreen(Vector3 targetScreenPoint)
    {
        pointerImage.sprite = crossSprite;
        pointerImage.rectTransform.localEulerAngles = Vector3.zero;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasTransform,
            targetScreenPoint,
            null,
            out Vector2 localPos
        );
        arrowSystemRect.anchoredPosition = localPos;
    }

    private void RotatePointerTowardsTarget(Vector3 targetScreenPoint)
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 direction = (Vector2)targetScreenPoint - screenCenter;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        pointerImage.rectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private void HandleMissionLocked()
    {
        pointerImage.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(true);
        _missionTimer = mission.LockTimer;
        _isMissionCooldown = true;
    }

    private void HandleMissionUnlocked()
    {
        pointerImage.gameObject.SetActive(true);
        cooldownText.gameObject.SetActive(false);
        _isMissionCooldown = false;
    }
}