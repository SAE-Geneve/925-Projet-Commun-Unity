using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ArrowPointer : MonoBehaviour
{
    [Header("Positioning")]
    public List<Transform> missionPositions;
    //1 - Border Control
    //2 - Boarding
    //3 - Conveyor Belt
    //4 - Lost Luggage
    //This is hard-coded so poorly :crying_emoji:
    [SerializeField] private RectTransform canvasTransform;
    
    [Header("Images")]
    [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;
    
    private Camera _mainCamera;
    private MissionManager missionManager;
    
    [SerializeField] private Image _pointerImage;
    [FormerlySerializedAs("_pointerRectTransform")] [SerializeField] private RectTransform pointerRectTransform;
    
    private Vector3 _targetPosition;

    private void Start()
    {
        _mainCamera = Camera.main;
        missionManager = FindObjectOfType<MissionManager>();
    }

    private void Update()
    {
        switch (missionManager.missionIndex)
        {
            case 0:
                _targetPosition = missionPositions[0].position;
                break;
            case 1:
                _targetPosition = missionPositions[1].position;
                break;
            case 2:
                _targetPosition = missionPositions[2].position;
                break;
            case 3:
                _targetPosition = missionPositions[3].position;
                break;
        }
        
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
            pointerRectTransform.anchoredPosition = localPos;
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
            pointerRectTransform.anchoredPosition = localPos;
            pointerRectTransform.localEulerAngles = Vector3.zero;
        }
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