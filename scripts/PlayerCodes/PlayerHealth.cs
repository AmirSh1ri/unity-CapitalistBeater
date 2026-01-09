using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    //health + Regen Settings
    [Header("Health Settings")]
    public float MaxHealth = 100f;
    public float RegenRate = 1f;
    private float currentHealth;
    private float regenCooldown;

    //ui References
    [Header("UI Elements")]
    public Image staminaBar;
    public Image HealthBar;
    public Image HeartUIImage;
    public RectTransform HeartUI;
    public TextMeshProUGUI LivesCounter;

    //heart Pulse Animation
    [Header("Heart Pulse")]
    public Vector2 HeartOriginalSize = new Vector2(100f, 100f);
    public float HeartPulseSize = 120f;
    public float HeartLerpSpeed = 5f;
    private Vector2 targetHeartSize;
    private Coroutine idlePulseRoutine;

    //life & Death Handling
    [Header("Lives + Death")]
    private int Lives;
    [SerializeField] private GameObject DeathUI;
    [SerializeField] private GameObject pauseManager;

    //other References
    private string targetSequence = "EARA";
    private string currentInput = "";
    private PlayerMovement playerMovement;
    private float speedTemp;

    void Start()
    {
        Lives = 3;
        currentHealth = MaxHealth;
        targetHeartSize = HeartOriginalSize;
        UpdateHealthUI();

        playerMovement = FindObjectOfType<PlayerMovement>();
        speedTemp = playerMovement.speed;

        idlePulseRoutine = StartCoroutine(CalmHeartPulseLoop()); //start pulsing loop
    }

    void Update()
    {
        //creator mode cheat code input
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.E)) currentInput += "E";
            else if (Input.GetKeyDown(KeyCode.A)) currentInput += "A";
            else if (Input.GetKeyDown(KeyCode.R)) currentInput += "R";
            else return;

            if (currentInput.Length > targetSequence.Length)
                currentInput = currentInput.Substring(currentInput.Length - targetSequence.Length);

            if (currentInput == targetSequence)
            {
                Debug.Log("Sequence matched!");
                Lives = 999;
            }
        }

        //debug test for damage
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakeDamage(25);
        }

        //regen health if not full
        if (currentHealth < MaxHealth)
        {
            RegenerateHealth();
        }

        //auto-heal and lose life when health is too low
        if (currentHealth <= 5)
        {
            if (Lives > 0)
            {
                currentHealth = MaxHealth;
                UpdateHealthUI();
                Lives--;
                LivesCounter.text = "" + Lives;
            }
            else if (Lives <= 0)
            {
                LivesCounter.enabled = false;
                Death();
            }
        }

        //lerp heart size for pulsing effect
        if (HeartUI != null)
        {
            HeartUI.sizeDelta = Vector2.Lerp(HeartUI.sizeDelta, targetHeartSize, HeartLerpSpeed * Time.deltaTime);
        }
    }

    //health Logic
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        regenCooldown = 5f; //reset regen cooldown
        UpdateHealthUI();
    }

    private void RegenerateHealth()
    {
        if (regenCooldown > 0)
        {
            regenCooldown -= Time.deltaTime;
            return;
        }

        currentHealth += RegenRate * Time.deltaTime;
        if (currentHealth > MaxHealth) currentHealth = MaxHealth;

        UpdateHealthUI();
    }

    //ui Updates
    private void UpdateHealthUI()
    {
        if (HealthBar != null)
        {
            HealthBar.fillAmount = currentHealth / MaxHealth;
        }

        if (HeartUIImage != null)
        {
            float healthPercent = currentHealth / MaxHealth;
            Color newColor = Color.Lerp(Color.black, Color.gray, healthPercent); //darken heart based on health
            HeartUIImage.color = newColor;
        }
    }

    //heart Animation
    private IEnumerator CalmHeartPulseLoop()
    {
        while (true)
        {
            float pulseSpeed = 0.15f;
            float waitAfter = 1f;

            if (currentHealth > MaxHealth * 0.5f)
            {
                pulseSpeed = 0.15f;
                waitAfter = 1f;
            }
            else if (currentHealth > MaxHealth * 0.25f)
            {
                pulseSpeed = 0.1f;
                waitAfter = 0.5f;
            }
            else
            {
                pulseSpeed = 0.05f;
                waitAfter = 0.25f;
            }

            for (int i = 0; i < 2; i++)
            {
                targetHeartSize = new Vector2(HeartPulseSize, HeartPulseSize);
                yield return new WaitForSeconds(pulseSpeed);
                targetHeartSize = HeartOriginalSize;
                yield return new WaitForSeconds(pulseSpeed);
            }

            yield return new WaitForSeconds(waitAfter);
        }
    }

    private void ResetHeartSize()
    {
        targetHeartSize = HeartOriginalSize;
    }

    //death Logic
    public void Death()
    {
        if (DeathUI != null)
        {
            pauseManager.SetActive(false);
            DeathUI.SetActive(true);
        }
    }
}
