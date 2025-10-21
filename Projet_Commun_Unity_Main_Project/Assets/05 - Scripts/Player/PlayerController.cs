using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    public PlayerInput Input { get; private set;}

    protected override void Start()
    {
        Input = GetComponent<PlayerInput>();
        base.Start();
    }
}