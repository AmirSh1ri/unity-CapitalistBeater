//makes glove camera and flashlight copy player's rotation

using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target != null)
        {
            //copy rotation
            transform.rotation = target.rotation;
        }
    }
}
