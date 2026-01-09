using UnityEngine;
using System.Collections;

//temporarily alters player's gravity and enables a visual effect

public class PowerupReceiver2 : MonoBehaviour
{
    public PlayerMovement player;
    public float alteredGravity = -90f;
    public float duration = 3f;
    public GameObject gravEffect;

    public void ActivatePower()
    {
        if (player != null)
        {
            StartCoroutine(AlterGravity());
        }
    }

    private IEnumerator AlterGravity()
    {
        //store original gravity
        float originalGravity = player.gravity;

        //apply gravity change and effect
        player.gravity = alteredGravity;
        gravEffect.SetActive(true);

        yield return new WaitForSeconds(duration);

        //revert gravity and disable effect
        player.gravity = originalGravity;
        gravEffect.SetActive(false);
    }
}
