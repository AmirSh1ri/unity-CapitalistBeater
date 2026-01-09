using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//diables grounded check for jumping

public class ForceAirStateQ : MonoBehaviour
{
    public float cooldown = 3f;
    public Image QABbar;

    private bool canUse = true;
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canUse && playerMovement != null)
        {
            StartCoroutine(LeapRoutine());
        }
    }

    private IEnumerator LeapRoutine()
    {
        canUse = false;

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

        //force air state briefly
        float t2 = 0f;
        while (t2 < 1f)
        {
            t2 += Time.deltaTime;
            playerMovement.ignoreGroundSnap = true;
            playerMovement.isGrounded = true;
            yield return null;
        }

        playerMovement.ignoreGroundSnap = false;

        //refill cooldown bar smoothly
        float t3 = 0f;
        while (t3 < cooldown)
        {
            t3 += Time.deltaTime;
            if (QABbar != null) QABbar.fillAmount = Mathf.Clamp01(t3 / cooldown);
            yield return null;
        }

        if (QABbar != null) QABbar.fillAmount = 1f;
        canUse = true;
    }
}
