using UnityEngine;

//triggers a powerup when the player collides with this object.
public class PowerupTrigger2 : MonoBehaviour
{
    public PowerupReceiver2 receiver;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && receiver != null)
        {
            receiver.ActivatePower();
            Destroy(gameObject);       //remove the powerup object after use
        }
    }
}
