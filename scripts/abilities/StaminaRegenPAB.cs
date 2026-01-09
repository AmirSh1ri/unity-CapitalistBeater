//boosts stamina regen when the obj is enabled

using UnityEngine;

public class StaminaRegenBoostPAB : MonoBehaviour
{
    public float percent = 20f;

    void Start()
    {
        var move = FindObjectOfType<PlayerMovement>(); //get movement
        if (move != null)
        {
            move.ChargeRate += move.ChargeRate * (percent / 100f); //boost regen
        }
    }
}
