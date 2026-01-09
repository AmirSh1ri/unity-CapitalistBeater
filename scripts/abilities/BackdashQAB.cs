using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// backdash ability triggered with Q, includes cooldown and UI bar
/// </summary>
public class BackdashAbilityE : MonoBehaviour
{
    //settings
    public float dashDistance = 7f;
    public float cooldown = 2f;
    public Image QABbar;

    private bool canDash = true;
    private CharacterController controller;

    void Start() => controller = GameObject.FindWithTag("Player").GetComponent<CharacterController>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canDash)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;

        //drain UI bar for ability Q
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

        controller.Move(-controller.transform.forward * dashDistance);

        yield return new WaitForSeconds(cooldown);

        //refill UI bar over cooldown duration
        if (QABbar != null)
        {
            float t = 0f;
            while (t < cooldown)
            {
                t += Time.deltaTime;
                QABbar.fillAmount = Mathf.Clamp01(t / cooldown);
                yield return null;
            }
            QABbar.fillAmount = 1f;
        }

        canDash = true;
    }
}
