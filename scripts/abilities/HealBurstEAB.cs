using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// heals the player and triggers cooldown with UI bar
/// </summary>

public class HealPulseAbilityE : MonoBehaviour
{
    public float healAmount = 30f;
    public float cooldown = 3f;
    public Image EABbar;

    private bool canUse = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canUse)
        {
            StartCoroutine(HealSequence());
        }
    }

    private IEnumerator HealSequence()
    {
        canUse = false;

        //apply healing as negative damage
        var health = FindObjectOfType<PlayerHealth>();
        if (health != null) health.TakeDamage(-healAmount);

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

        //refill cooldown bar smoothly
        float t2 = 0f;
        while (t2 < cooldown)
        {
            t2 += Time.deltaTime;
            if (EABbar != null) EABbar.fillAmount = Mathf.Clamp01(t2 / cooldown);
            yield return null;
        }

        if (EABbar != null) EABbar.fillAmount = 1f;
        canUse = true;
    }
}
