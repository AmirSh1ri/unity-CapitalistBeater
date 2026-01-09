//manages acid UI and damage effects

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AcidLevel : MonoBehaviour
{
    public Transform groundCheck;
    public float checkRadius = 0.2f;
    public LayerMask acidLayer;
    public Image barImage;
    public GameObject AcidDis;
    public float maxValue = 100f;
    public float pulseIncrease = 25f;
    public float decreaseAmount = 10f;
    public float lerpSpeed = 5f;
    public float acidDamage = 5f;

    private float currentValue = 0f;
    private float targetValue = 0f;
    private bool isTouchingAcid = false;
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>(); //get player health script
        StartCoroutine(AcidPulse()); //start acid pulse loop
    }

    void Update()
    {
        if (IsTouchingAcid())
        {
            isTouchingAcid = true;
            ToggleUI(true); //show UI when touching acid
        }
        else
        {
            isTouchingAcid = false;
            targetValue -= decreaseAmount * Time.unscaledDeltaTime;
        }

        targetValue = Mathf.Clamp(targetValue, 0f, maxValue);
        currentValue = Mathf.Lerp(currentValue, targetValue, lerpSpeed * Time.unscaledDeltaTime);

        if (barImage != null)
        {
            barImage.fillAmount = currentValue / maxValue;
        }

        if (AcidDis != null && currentValue <= 5f)
        {
            AcidDis.SetActive(false); //hide UI when level is low
        }
    }

    IEnumerator AcidPulse()
    {
        while (true)
        {
            if (isTouchingAcid)
            {
                targetValue += pulseIncrease;
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(Mathf.Lerp(0, acidDamage * 2, 1f)); //apply normal acid damage
                }
                if (currentValue >= 90f)
                {
                    playerHealth.TakeDamage(Mathf.Lerp(0, acidDamage * 10, 1f)); //extra damage at high level
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    bool IsTouchingAcid()
    {
        return Physics.CheckSphere(groundCheck.position, checkRadius, acidLayer);
    }

    void ToggleUI(bool state)
    {
        if (AcidDis != null && state)
        {
            AcidDis.SetActive(true);
        }
    }
}
