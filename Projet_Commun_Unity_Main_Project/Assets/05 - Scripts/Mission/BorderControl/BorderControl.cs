using System.Diagnostics;
using UnityEngine;

public class BorderControl : MonoBehaviour
{
    [SerializeField] private GameObject addedConveyor1;
    [SerializeField] private GameObject addedConveyor2;
    [SerializeField] private DirectionSwitch directionSwitch1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        switch (PlayerManager.Instance.PlayerCount)
        {
            case 3:
                addedConveyor1.SetActive(true);
                addedConveyor2.SetActive(false);
                directionSwitch1.SetDirection(1);
                break;
            case 4:
                addedConveyor1.SetActive(true);
                addedConveyor2.SetActive(true);
                directionSwitch1.SetDirection(1);
                break;
            default:
                addedConveyor1.SetActive(false);
                addedConveyor2.SetActive(false);
                directionSwitch1.SetDirection(0);
                break;
        }
    }
}
