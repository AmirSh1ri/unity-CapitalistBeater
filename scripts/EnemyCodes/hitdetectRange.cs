// detects collision with player and applies damage if hit

using UnityEngine;

public class hitdetectRange : MonoBehaviour
{
    // checks for player collision and deals damage
    void OnTriggerEnter(Collider other)
    {
        PlayerHealth hp = other.GetComponent<PlayerHealth>();
        if (hp != null)
        {
            hp.TakeDamage(10f);
            Destroy(gameObject);
        }
    }
}
