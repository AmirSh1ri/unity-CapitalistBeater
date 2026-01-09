//adjusts heart icon brightness based on health bar

using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public Image heartImage;
    public Image healthBar;

    void Update()
    {
        if (healthBar != null && heartImage != null)
        {
            float fillAmount = healthBar.fillAmount;
            float brightness = Mathf.Clamp(fillAmount, 0f, 1f); //set brightness based on health
            heartImage.color = new Color(brightness, brightness, brightness, 1f);
        }
    }
}
