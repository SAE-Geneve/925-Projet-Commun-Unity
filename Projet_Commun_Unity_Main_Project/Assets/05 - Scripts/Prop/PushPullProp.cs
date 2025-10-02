using UnityEngine;

public class PushPullProp : MonoBehaviour
{
    private Transform player;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    
    public virtual void Grabbed(Transform grabber)
    {
        player = grabber;
    }

    public virtual void Dropped()
    {
        player = null;
    }
}
