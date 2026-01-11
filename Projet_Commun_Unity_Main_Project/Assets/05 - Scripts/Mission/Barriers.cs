using System.Collections.Generic;
using UnityEngine;

public class Barriers : MonoBehaviour
{
    [SerializeField] private List<GameObject> barriers_ = new List<GameObject>();

    public void ActivateBarriers()
    {
        foreach (var barrier in barriers_)
        {
            barrier.gameObject.SetActive(true);
        }
    }
    
    public void DesactivateBarriers()
    {
        foreach (var barrier in barriers_)
        {
            barrier.gameObject.SetActive(false);
        }
    }
}
