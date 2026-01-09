using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//fills ult to 75% and triggers cooldown on E press

public class PartialUltFillE : MonoBehaviour
{
    public Image ultBar;      //ult meter (fills to 75%)
    public Image EABbar;      //cooldown bar
    public float cooldown = 3f;

    private bool canUse = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canUse && ultBar != null)
        {
            StartCoroutine(PerformFadeStep());
        }
    }

    private IEnumerator PerformFadeStep()
    {
        canUse = false;

        //set ult to 75%
        ultBar.fillAmount = 0.75f;

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
