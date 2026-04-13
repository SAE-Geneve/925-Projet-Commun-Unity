using UnityEngine;
using UnityEngine.UI;

public class MissionPointer : MonoBehaviour
{
    [Header("Position")]
    [SerializeField] private Mission mission;
    
    [Header("Images")]
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;
    
    [Header("Arrow References")]
    [SerializeField] private Image pointerImage;
    [SerializeField] private RectTransform pointerRectTransform;
    
    private Camera _mainCamera;
    private Vector3 _targetPosition;
    private RectTransform _canvasTransform;

    private void Start()
    {
        _mainCamera = Camera.main;
        _targetPosition=mission.gameObject.transform.position;
        _canvasTransform=GetComponent<RectTransform>();
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
