//handles dash move with cooldown and ui bar

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DashAbility : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Camera mainCamera;

    public Image EABbar;
    public GameObject wholeEABUI;
    public float dashSpeed = 50f;
    public float dashDuration = 1f;
    public float dashCooldown = 3f;
    public float barEmptySpeed = 4f;

    private bool canDash = true;
    private float originalFOV;
    public float dashFOV = 100f;

    void Start()
    {
        if (mainCamera != null)
        {
            originalFOV = mainCamera.fieldOfView;
        }
    }

    void Update()
    {
        if (playerMovement == null || mainCamera == null) return;

        if (Input.GetKeyDown(KeyCode.E) && canDash)
        {
            StartCoroutine(PerformDashAndCooldown());
        }
    }

    private IEnumerator PerformDashAndCooldown()
    {
        canDash = false;

        //show bar once
        if ( wholeEABUI != null)
        {
            wholeEABUI.SetActive(true);
        }

        //drain bar
        if (EABbar != null)
        {
            float t = 0f;
            float startFill = EABbar.fillAmount;
            while (t < 1f)
            {
                t += Time.deltaTime * barEmptySpeed;
                EABbar.fillAmount = Mathf.Lerp(startFill, 0f, t);
                yield return null;
            }
        }

        //do dash
        float originalPlayerSpeed = playerMovement.speed;
        playerMovement.speed = dashSpeed;
        mainCamera.fieldOfView = dashFOV;

        yield return new WaitForSeconds(dashDuration);

        //reset after dash
        playerMovement.speed = originalPlayerSpeed;
        mainCamera.fieldOfView = originalFOV;

        //cooldown refill
        if (EABbar != null)
        {
            float t = 0f;
            while (t < dashCooldown)
            {
                t += Time.deltaTime;
                EABbar.fillAmount = Mathf.Clamp01(t / dashCooldown);
                yield return null;
            }
            EABbar.fillAmount = 1f;
        }

        canDash = true;
    }
}
