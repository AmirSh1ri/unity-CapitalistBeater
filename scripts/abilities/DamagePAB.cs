//boosts player attack damage by a percent on start

using UnityEngine;

public class DamagePAB : MonoBehaviour
{
    public float damageBoostPercent = 5f;

    void Start()
    {
        Attack attack = FindObjectOfType<Attack>();
        if (attack != null)
        {
            float bonus = attack.attackDamage * (damageBoostPercent / 100f); //calc bonus
            attack.attackDamage += Mathf.RoundToInt(bonus); //add bonus
        }
    }
}
