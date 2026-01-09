using UnityEngine;

//this script restores a random amount of stamina to the player upon pickup

public class StaminaPickup : MonoBehaviour
{
    public float minPercent = 0.5f;
    public float maxPercent = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                float percent = Random.Range(minPercent, maxPercent);
                player.Stamina = player.MaxStamina * percent;
                player.StaminaBar.fillAmount = player.Stamina / player.MaxStamina; 
            }

            Destroy(gameObject); 
        }
    }
}
