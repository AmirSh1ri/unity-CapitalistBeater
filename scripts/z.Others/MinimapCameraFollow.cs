// keeps the minimap camera above the player at a fixed height and rotation

using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 20f, 0);

    // positions and orients the minimap camera every frame
    void LateUpdate()
    {
        transform.position = player.position + offset;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }
}
