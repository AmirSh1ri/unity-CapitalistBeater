using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// smoothly rewinds the player to a saved position after a delay, with cooldown
/// </summary>
public class SmoothRewindE : MonoBehaviour
{
    public RawImage rawImageUI;
    public Transform player;
    public float delay = 3f;
    public float rewindDuration = 1f;
    public float cooldown = 3f;
    public Image EABbar;

    private bool canUse = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canUse)
        {
            StartCoroutine(RewindAfterDelay());
        }
    }

    private IEnumerator RewindAfterDelay()
    {
        canUse = false;
        Color originalColor = rawImageUI.color;

        //drain cooldown bar quickly
        if (EABbar != null)
        {
            float t = 0f;
            float startFill = EABbar.fillAmount;
            while (t < 1f)
            {
                t += Time.deltaTime * 4f;
                EABbar.fillAmount = Mathf.Lerp(startFill, 0f, t);
                yield return null;
            }
        }

        //save position and wait
        Vector3 savedPosition = player.position;
        yield return new WaitForSeconds(delay);

        //rewind to saved position over time
        Vector3 start = player.position;
        float t2 = 0f;
        while (t2 < 1f)
        {
            rawImageUI.color = Color.blue;
            t2 += Time.deltaTime / rewindDuration;
            player.position = Vector3.Lerp(start, savedPosition, t2);
            yield return null;
        }

        rawImageUI.color = originalColor;
        player.position = savedPosition;

        //refill cooldown bar smoothly
        float t3 = 0f;
        while (t3 < cooldown)
        {
            t3 += Time.deltaTime;
            if (EABbar != null) EABbar.fillAmount = Mathf.Clamp01(t3 / cooldown);
            yield return null;
        }

        if (EABbar != null) EABbar.fillAmount = 1f;
        canUse = true;
    }
}
