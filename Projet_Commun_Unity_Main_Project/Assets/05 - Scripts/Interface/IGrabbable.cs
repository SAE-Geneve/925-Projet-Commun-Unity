
using UnityEngine;

public interface IGrabbable
{
    void Grabbed(Controller controller);

    void Dropped(Vector3 throwForce = default, Controller controller = null);
}
