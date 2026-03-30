using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowPointer : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;

    private Camera _mainCamera;
    private MissionManager _missionManager;

    [Header("Components")]
    [SerializeField] private Image pointerImage;
    [SerializeField] private Image distanceBox;
    [SerializeField] private RectTransform pointerRectTransform;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI cooldownText;

    private Vector3 _targetPosition;

    private bool _isMissionCooldown;
    private float _hubTimer;

    private void Start()
    {
        _mainCamera = Camera.main;
        _missionManager = MissionManager.Instance;
        _isMissionCooldown = false;
        
        
        _missionManager.OnMissionReady += HandleMissionReady;
        _missionManager.OnMissionCooldownStarted += HandleMissionCooldownStarted;
    }

    private void Update()
    {
        if(!_isMissionCooldown)
        {
            _targetPosition = _missionManager.CurrentMission.ArrowTarget.position;

            RotatePointerTowardsTarget();
            CalculateDistance();
        }
        else
        {
            _hubTimer -= Time.deltaTime;
            cooldownText.text = (int)_hubTimer+"";
        }
    }

    private void HandleMissionReady()
    {
        _isMissionCooldown = false;
        distanceBox.gameObject.SetActive(true);
        pointerImage.gameObject.SetActive(true);
        cooldownText.gameObject.SetActive(false);
    }

    private void HandleMissionCooldownStarted()
    {
        _isMissionCooldown = true;
        distanceBox.gameObject.SetActive(false);
        pointerImage.gameObject.SetActive(false);
        cooldownText.gameObject.SetActive(true);
        _hubTimer = _missionManager.GetHubTime();
    }

    private void CalculateDistance()
    {
        Vector3 targetPositionScreenPoint = _mainCamera.WorldToScreenPoint(_targetPosition);
        
        float distance = Vector3.Distance(_mainCamera.transform.position, targetPositionScreenPoint)/10f;
        
        distanceText.text = (int)distance+"M";
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