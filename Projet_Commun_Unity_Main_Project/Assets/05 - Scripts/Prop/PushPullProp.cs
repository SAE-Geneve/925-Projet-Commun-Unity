using UnityEngine;

public class PushPullProp : Prop
{
    private Transform player;

    public override void Grabbed(Catcher catcher)
    {
        // player = grabber;
    }

    public override void Dropped(Vector3 throwForce = default)
    {
        player = null;
    }
}
