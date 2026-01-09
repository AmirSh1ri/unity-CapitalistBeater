// makes the object move toward the player until within a stopping distance

using UnityEngine;

public class MoveToPlayer : MonoBehaviour
{   
    public Transform player;
    public float speed = 5f;
    public float stopDistance = 1.5f;

    // checks distance and moves toward the player if too far
    void Update()
    {
        if (player == null) return;

        // get direction and distance to player
        Vector3 direction = player.position - transform.position;
        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            // move toward player
            Vector3 moveDir = direction.normalized;
            transform.position += moveDir * speed * Time.deltaTime;

            // face the player
            transform.forward = moveDir;
        }
    }
}
