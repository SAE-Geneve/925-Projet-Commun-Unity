using UnityEngine;

public class Mission : MonoBehaviour
{
    [Header("Parameters")] 
    [SerializeField] private string _name = "New Mission";
    [SerializeField] private float _initialTime = 30f;
    [SerializeField] private bool _locked;
}
