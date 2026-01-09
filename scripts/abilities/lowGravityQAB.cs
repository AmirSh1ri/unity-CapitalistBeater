using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// temporarily lowers gravity for a set duration and triggers cooldown
/// </summary>

public class LowGravityAbilityE : MonoBehaviour
{
    public float lowGravity = -10f;
    public float duration = 3f;
    public float cooldown = 3f;
    public Image QABbar;

    private float originalGravity;
    private bool canUse = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canUse)
        {
            var move = FindObjectOfType<PlayerMovement>();
            if (move != null)
            {
                StartCoroutine(ActivateLowGravity(move));
            }
        }
    }

    private IEnumerator ActivateLowGravity(PlayerMovement move)
    {
        canUse = false;

        //apply low gravity
        originalGravity = move.gravity;
        move.gravity = lowGravity;

        //drain cooldown bar quickly
        if (QABbar != null)
        {
            float t = 0f;
            float startFill = QABbar.fillAmount;
            while (t < 1f)
            {
                t += Time.deltaTime * 4f;
                QABbar.fillAmount = Mathf.Lerp(startFill, 0f, t);
                yield return null;
            }
        }

        //wait for duration then reset gravity
        yield return new WaitForSeconds(duration);
        move.gravity = originalGravity;

        //refill cooldown bar smoothly
        float t2 = 0f;
        while (t2 < cooldown)
        {
            t2 += Time.deltaTime;
            if (QABbar != null) QABbar.fillAmount = Mathf.Clamp01(t2 / cooldown);
            yield return null;
        }

        if (QABbar != null) QABbar.fillAmount = 1f;
        canUse = true;
    }
}
