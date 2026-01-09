using UnityEngine;

public class PowerupTrigger : MonoBehaviour
{
    public PowerupReceiver receiver;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && receiver != null)
        {
            receiver.ActivatePower();
            Destroy(gameObject);
        }
    }
}
