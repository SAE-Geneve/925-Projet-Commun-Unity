using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ArrowPointer : MonoBehaviour
{
    [Header("Positioning")]
    public List<Transform> missionPositions;
    [SerializeField] private RectTransform canvasTransform;
    
    [Header("Images")]
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;
    
    private Camera _mainCamera;
    
    private Image _pointerImage;
    private RectTransform _pointerRectTransform;
    
    private Vector3 _targetPosition;

    private void Start()
    {
        _mainCamera = Camera.main;
        
        _pointerRectTransform = gameObject.GetComponent<RectTransform>();
        
        MissionID missionID = UIManager.Instance.TargetMission;
        _targetPosition = missionPositions[(int)missionID].position;
        
        _pointerImage = gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        Vector3 targetPositionScreenPoint = _mainCamera.WorldToScreenPoint(_targetPosition);
        
        //Checks if the target is in the screen
        float borderSize = 100f;
        bool isOffScreen = targetPositionScreenPoint.x < borderSize ||
                           targetPositionScreenPoint.x > Screen.width - borderSize||
                           targetPositionScreenPoint.y < borderSize ||
                           targetPositionScreenPoint.y > Screen.height - borderSize;

        if (isOffScreen)
        {
            RotatePointerTowardsTarget();
            _pointerImage.sprite = arrowSprite;
            //Bring back the arrow in the screen if it is out of the border
            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            cappedTargetScreenPosition.x =
                Mathf.Clamp(cappedTargetScreenPosition.x, borderSize, Screen.width - borderSize);
            cappedTargetScreenPosition.y =
                Mathf.Clamp(cappedTargetScreenPosition.y, borderSize, Screen.height - borderSize);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasTransform,
                cappedTargetScreenPosition,
                null,
                out Vector2 localPos
            );
            _pointerRectTransform.anchoredPosition = localPos;
        }
        else
        {
            _pointerImage.sprite = crossSprite;
            //If on screen
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasTransform,
                targetPositionScreenPoint,
                null,
                out Vector2 localPos
            );
            _pointerRectTransform.anchoredPosition = localPos;
            _pointerRectTransform.localEulerAngles = Vector3.zero;
        }
    }

    private void RotatePointerTowardsTarget()
    {
        Vector3 targetPositionScreenPoint = _mainCamera.WorldToScreenPoint(_targetPosition);
        
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Vector2 direction = (Vector2)targetPositionScreenPoint - screenCenter;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360;
        _pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}