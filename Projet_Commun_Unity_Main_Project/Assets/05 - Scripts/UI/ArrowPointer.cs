using UnityEngine;

public class ArrowPointer : MonoBehaviour
{
    private Camera uiCamera;
    private Vector3 _targetPosition;
    private RectTransform _pointerRectTransform;

    private void Awake()
    {
        uiCamera = Camera.main;
        _targetPosition = new Vector3(30f, 0.3f, 0f);
        _pointerRectTransform = gameObject.GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 toPosition = _targetPosition;
        Vector3 fromPosition = Camera.main.transform.position;
        fromPosition.z = 0f;
        Vector3 direction = (toPosition - fromPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        _pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);

        float borderSize = 100f;
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(_targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= borderSize ||
                           targetPositionScreenPoint.x >= Screen.width - borderSize ||
                           targetPositionScreenPoint.y <= borderSize ||
                           targetPositionScreenPoint.y >= Screen.height - borderSize;

        if (isOffScreen)
        {
            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= borderSize) cappedTargetScreenPosition.x = borderSize;
            if (cappedTargetScreenPosition.x >= Screen.width - borderSize)
                cappedTargetScreenPosition.x = Screen.width - borderSize;
            if (cappedTargetScreenPosition.y <= borderSize) cappedTargetScreenPosition.y = borderSize;
            if (cappedTargetScreenPosition.y >= Screen.height - borderSize)
                cappedTargetScreenPosition.y = Screen.height - borderSize;

            Vector3 pointerWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
            _pointerRectTransform.position = pointerWorldPosition;
            _pointerRectTransform.localPosition = new Vector3(_pointerRectTransform.localPosition.x,
                _pointerRectTransform.localPosition.y, 0f);
        }
    }
}