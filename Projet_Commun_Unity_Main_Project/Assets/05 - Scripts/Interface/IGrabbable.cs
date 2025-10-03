

using UnityEngine;

public interface IGrabbable
{
    void Grabbed(Transform grabber);

    void Dropped();
}
