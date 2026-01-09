using UnityEngine;

//heals the player for a random amount when picked up.
public class HealthPickup : MonoBehaviour
{
    public float minHeal = 25f;
    public float maxHeal = 75f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                float healAmount = Random.Range(minHeal, maxHeal);
                health.TakeDamage(-healAmount);
            }

            Destroy(gameObject);  // remove pickup after use
        }
    }
}
