using UnityEngine;

public class PropGrabTask : Prop
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void Grabbed(Controller controller)
    {
        GameManager.Instance.CurrentMission.Finish();
    }
}
