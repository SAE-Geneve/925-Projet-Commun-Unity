using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowPointer : MonoBehaviour
{
    [Header("Positioning")] public List<Transform> missionPositions;
    //1 - Border Control
    //2 - Boarding
    //3 - Conveyor Belt
    //4 - Lost Luggage
    //This is hard-coded so poorly :crying_emoji:

    [Header("Images")] [SerializeField] private Sprite arrowSprite;
    [SerializeField] private Sprite crossSprite;

    private Camera _mainCamera;
    private MissionManager _missionManager;

    [Header("Components")] [SerializeField]
    private Image pointerImage;

    [SerializeField] private RectTransform pointerRectTransform;
    [SerializeField] private TextMeshProUGUI distanceText;

    private Vector3 _targetPosition;

    private void Start()
    {
        _mainCamera = Camera.main;
        _missionManager = FindObjectOfType<MissionManager>();
    }

    private void Update()
    {
        pointerImage.gameObject.SetActive(!_missionManager.currentMission.IsLocked);

        _targetPosition = _missionManager.currentMission.ArrowTarget.position;

        RotatePointerTowardsTarget();
        CalculateDistance();
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