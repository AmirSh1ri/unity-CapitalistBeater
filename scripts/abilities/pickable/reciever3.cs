using UnityEngine;

public class PowerupTrigger3 : MonoBehaviour
{
    public PowerupReceiver3 receiver;

    //activates powerup when player touches the trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && receiver != null)
        {
            receiver.ActivatePower();
            Destroy(gameObject);
        }
    }
}
