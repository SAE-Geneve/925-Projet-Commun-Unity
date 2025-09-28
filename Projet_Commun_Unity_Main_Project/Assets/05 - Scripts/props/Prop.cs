using UnityEngine;

public class Grabbable : MonoBehaviour, IGrabbable
{
    private Rigidbody _rb;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Grabbable()
    {
        throw new System.NotImplementedException();
    }

    public void Dropped()
    {
        throw new System.NotImplementedException();
    }
}
