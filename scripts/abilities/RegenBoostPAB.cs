using UnityEngine;

public class RegenBoostPAB : MonoBehaviour
{
    public float percent = 25f;

    //boost regen rate by percent
    void Start()
    {
        var health = FindObjectOfType<PlayerHealth>();
        if (health != null)
        {
            health.RegenRate += health.RegenRate * (percent / 100f);
        }
    }
}
