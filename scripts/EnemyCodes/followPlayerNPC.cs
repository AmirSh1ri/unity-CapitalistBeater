// rotates enemy to face the player horizontally

using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;

    // rotates toward the player every frame
    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;

        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
