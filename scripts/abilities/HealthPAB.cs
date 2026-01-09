// boosts player's max health by a percentage when activated

using UnityEngine;

public class HealthPAB : MonoBehaviour
{
    public float healthBoostPercent = 10f;

    // increases player's max health by given percent
    void Start()
    {
        PlayerHealth health = FindObjectOfType<PlayerHealth>();
        if (health != null)
        {
            float boostAmount = health.MaxHealth * (healthBoostPercent / 100f);
            health.MaxHealth += boostAmount;
        }
    }
}
