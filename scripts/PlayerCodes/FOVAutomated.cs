// syncs glove camera FOV with main camera, adds slight boost if main FOV is high

using UnityEngine;

public class GloveCameraSync : MonoBehaviour
{
    public Camera mainCamera;
    public Camera gloveCamera;

    // adjusts glove camera FOV based on main camera
    void Update()
    {
        if (mainCamera.fieldOfView > 61f)
        {
            gloveCamera.fieldOfView = 67f;
        }
        else
        {
            gloveCamera.fieldOfView = mainCamera.fieldOfView;
        }
    }
}
