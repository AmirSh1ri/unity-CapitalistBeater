using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//temporarily boosts damage, attack speed, and animation speed

public class PowerupReceiver3 : MonoBehaviour
{
    public Attack attackScript;
    public RawImage rawImageUI;
    public GameObject DMGEffect;

    public void ActivatePower()
    {
        StartCoroutine(dmgBoost());
    }

    private IEnumerator dmgBoost()
    {
        //store original values
        Color originalColor = rawImageUI.color;
        float originalCooldown = attackScript.attackCooldown;
        int originalDamage = attackScript.attackDamage;
        float originalAnimSpeed = 1f;

        DMGEffect.SetActive(true);
        rawImageUI.color = Color.red;

        //speed up weapon animation if present
        if (attackScript.activeWeapon != null)
        {
            var anim = attackScript.activeWeapon.GetComponent<Animator>();
            if (anim != null)
            {
                originalAnimSpeed = anim.speed;
                anim.speed = 2f;
            }
        }

        //apply damage buff
        attackScript.attackCooldown = originalCooldown / 2f;
        attackScript.attackDamage = originalDamage * 3;

        yield return new WaitForSeconds(3f);

        //revert all changes
        rawImageUI.color = originalColor;
        attackScript.attackCooldown = originalCooldown;
        attackScript.attackDamage = originalDamage;
        DMGEffect.SetActive(false);

        if (attackScript.activeWeapon != null)
        {
            var anim = attackScript.activeWeapon.GetComponent<Animator>();
            if (anim != null) anim.speed = originalAnimSpeed;
        }
    }
}
