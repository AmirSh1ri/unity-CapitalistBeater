using UnityEngine;
using System.Collections;

//handles temporary player powerups like speed boosts and shows related UI.
public class PowerupReceiver : MonoBehaviour
{
    public PlayerMovement player;
    public GameObject speedEffect;

    public void ActivatePower()
    {
        Debug.Log("1");

        if (player != null)
        {
            StartCoroutine(SpeedBoost());
        }
    }

    private IEnumerator SpeedBoost()
    {
        float originalSpeed = player.speed;
        player.speed = 50f;

        speedEffect.SetActive(true); 

        yield return new WaitForSeconds(3f);

        player.speed = originalSpeed;
        speedEffect.SetActive(false);
    }
}
