// increases player max stamina by a percentage on start

using UnityEngine;

public class StaminaPAB : MonoBehaviour
{
    public float staminaBoostPercent = 10f; 

    void Start()
    {
        PlayerMovement movement = FindObjectOfType<PlayerMovement>(); 
        if (movement != null)
        {
            float boostAmount = movement.MaxStamina * (staminaBoostPercent / 100f);
            movement.MaxStamina += boostAmount; 
        }
    }
}
