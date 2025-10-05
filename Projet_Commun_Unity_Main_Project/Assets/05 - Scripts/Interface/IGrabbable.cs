
using UnityEngine;

public interface IGrabbable
{
    void Grabbed(Catcher catcher);

    void Dropped(Vector3 throwForce = default);
}
