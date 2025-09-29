using UnityEngine;

public class MovableProp : Prop
{
    public override void Grabbed(/*référence du catcher*/)
    {
        Debug.Log("Grabbed movable object");
    }

    public override void Dropped()
    {
        Debug.Log("Dropped movable object");
    }
}
