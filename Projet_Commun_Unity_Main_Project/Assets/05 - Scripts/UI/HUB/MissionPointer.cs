using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class MissionPointer : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private Mission mission;
    
    [Header("Arrow Sprites")]
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;
    [SerializeField] private Sprite missionSprite;
    
    [Header("Icon Images")]
    [SerializeField] private Image missionIcon;
    [SerializeField] private TextMeshProUGUI cooldownText;
    
    [Header("Arrow References")]
    [SerializeField] private Image pointerImage;
    [SerializeField] private RectTransform pointerRectTransform;
    
    private Camera _mainCamera;
    private Vector3 _targetPosition;
    private RectTransform _canvasTransform;
    private bool _isMissionCooldown;
    private float _missionTimer;

    private void Start()
    {
        _mainCamera = Camera.main;
        _targetPosition=mission.gameObject.transform.position;
        _canvasTransform=GetComponent<RectTransform>();
        missionIcon.sprite = missionSprite;
        cooldownText.gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 targetPositionScreenPoint = _mainCamera.WorldToScreenPoint(_targetPosition);
        
        //Checks if the target is in the screen
        float borderSize = 150f;
        bool isOffScreen = targetPositionScreenPoint.x < borderSize ||
                           targetPositionScreenPoint.x > Screen.width - borderSize||
                           targetPositionScreenPoint.y < borderSize+50 ||
                           targetPositionScreenPoint.y > Screen.height - borderSize-50;

        if (isOffScreen)
        {
            ArrowOffScreen(targetPositionScreenPoint, borderSize);
        }
        else
        {
            ArrowOnScreen(targetPositionScreenPoint);
        }

        //This is so bad, I tried events but it didn't work
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
            cooldownText.text = (int)_missionTimer+"";
        }
    }

    private void HandleMissionLocked()
    {
        missionIcon.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(true);
        _missionTimer = mission.Timer;
        _isMissionCooldown = true;
    }

    private void HandleMissionUnlocked()
    {
        missionIcon.gameObject.SetActive(true);
        cooldownText.gameObject.SetActive(false);
        _isMissionCooldown = false;
    }
    
    private void ArrowOffScreen(Vector3 targetPositionScreenPoint, float borderSize)
    {
        RotatePointerTowardsTarget();
        pointerImage.sprite = arrowSprite;
        //Bring back the arrow in the screen if it is out of the border
        Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
        cappedTargetScreenPosition.x =
            Mathf.Clamp(cappedTargetScreenPosition.x, borderSize, Screen.width - borderSize);
        cappedTargetScreenPosition.y =
            Mathf.Clamp(cappedTargetScreenPosition.y, borderSize, Screen.height - borderSize);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasTransform,
            cappedTargetScreenPosition,
            null,
            out Vector2 localPos
        );
        pointerRectTransform.anchoredPosition = localPos;
        UpdateIconPosition(localPos);
    }

    private void ArrowOnScreen(Vector3 targetPositionScreenPoint)
    {
        pointerImage.sprite = crossSprite;
        //If on screen
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasTransform,
            targetPositionScreenPoint,
            null,
            out Vector2 localPos
        );
        pointerRectTransform.anchoredPosition = localPos;
        pointerRectTransform.localEulerAngles = Vector3.zero;
        UpdateIconPosition(localPos);
    }

    private void UpdateIconPosition(Vector2 localPos)
    {
        float offsetDistance = 40f;
    
        float angle = (pointerRectTransform.localEulerAngles.z - 90f) * Mathf.Deg2Rad;
        Vector2 arrowDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    
        missionIcon.rectTransform.anchoredPosition = localPos + arrowDirection * offsetDistance;
        missionIcon.rectTransform.localEulerAngles = Vector3.zero;
    }
    
    private void RotatePointerTowardsTarget()
    {
        Vector3 targetPositionScreenPoint = _mainCamera.WorldToScreenPoint(_targetPosition);
        
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Vector2 direction = (Vector2)targetPositionScreenPoint - screenCenter;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360;
        pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}
