using UnityEngine;

public class FollowPlayerUI : MonoBehaviour
{
    public Transform player; 
    void Update()
    {
        if (player == null) return;

        transform.LookAt(player);
        transform.Rotate(0, 180, 0);
    }
}
