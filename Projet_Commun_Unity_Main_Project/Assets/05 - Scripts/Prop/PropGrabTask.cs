public class PropGrabTask : Prop
{
    public override void Grabbed(Controller controller)
    {
        GameManager.Instance.CurrentMission.Finish();
    }
}
