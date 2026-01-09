using UnityEngine;
using UnityEngine.UI;

//adds a random amount of charge to the player's ult bar on pickup.
public class UltPickup : MonoBehaviour
{
    public float minFill = 0.5f;
    public float maxFill = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Attack attack = other.GetComponent<Attack>();
            if (attack != null && attack.ultBar != null)
            {
                float charge = Random.Range(minFill, maxFill);
                attack.ultBar.fillAmount = Mathf.Clamp01(attack.ultBar.fillAmount + charge);
            }

            Destroy(gameObject);  //remove pickup after use
        }
    }
}
