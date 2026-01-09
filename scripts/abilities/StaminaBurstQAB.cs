using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// instantly refills stamina and triggers cooldown with UI bar
/// </summary>
public class StaminaPulseAbilityE : MonoBehaviour
{
    public float cooldown = 3f;
    public Image QABbar;

    private bool canUse = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canUse)
        {
            StartCoroutine(StaminaRoutine());
        }
    }

    private IEnumerator StaminaRoutine()
    {
        canUse = false;

        //refill stamina
        var move = FindObjectOfType<PlayerMovement>();
        if (move != null) move.RefillStamina();

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
