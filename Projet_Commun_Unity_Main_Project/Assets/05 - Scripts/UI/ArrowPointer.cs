using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ArrowPointer : MonoBehaviour
{
    [Header("Positioning")]
    public List<Transform> missionPositions;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private RectTransform canvasTransform;
    
    [Header("Images")]
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;
    
    private Image _pointerImage;
    private RectTransform _pointerRectTransform;

    private void Awake()
    {
        _pointerRectTransform = gameObject.GetComponent<RectTransform>();
  
        if (!GameManager.Instance.GetMission(MissionID.Boarding).IsLocked)
        {
            targetPosition.position = missionPositions[3].position;
        }
        else if (!GameManager.Instance.GetMission(MissionID.LostLuggage).IsLocked)
        {
            targetPosition.position = missionPositions[2].position;
        }
        else if (!GameManager.Instance.GetMission(MissionID.ConveyorBelt).IsLocked)
        {
            targetPosition.position = missionPositions[1].position;
        }
        else if (!GameManager.Instance.GetMission(MissionID.BorderControl).IsLocked)
        {
            targetPosition.position = missionPositions[0].position;
        }
        _pointerImage=gameObject.GetComponent<Image>();
    }

    private void Update()
    {
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition.position);
        
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
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition.position);
        
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Vector2 direction = (Vector2)targetPositionScreenPoint - screenCenter;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % 360;
        _pointerRectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }
}