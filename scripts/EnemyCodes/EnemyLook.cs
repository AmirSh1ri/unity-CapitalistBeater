// enemy script handles damage, death, screen shake, regeneration, and skull rewards

using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Camera mainCamera;
    public Transform player;
    private Animator animator;
    private CurrencyHolder currencyHolder;

    [SerializeField] private int health = 50;
    [SerializeField] private bool canRegenerate = false;
    [SerializeField] private int regenAmount = 5;
    private float regenRate = 0f;
    private bool isDead = false;

    public int skullsReward = 0;

    public ParticleSystem bloodEffect;
    public ParticleSystem deathEffect;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        animator = GetComponent<Animator>();
        currencyHolder = FindObjectOfType<CurrencyHolder>();

        if (canRegenerate)
            InvokeRepeating(nameof(RegenerateHealth), regenRate, regenRate);
    }

    void Update()
    {
        if (isDead) return;

        // face camera
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;
        directionToCamera.y = 0;
        transform.rotation = Quaternion.LookRotation(directionToCamera);
    }

    // called when enemy takes damage
    public void TakeDamage(int damage)
    {
        animator.SetBool("isHit", true);
        animator.Play("hit", 0, 0f);
        StartCoroutine(ResetHitAnimation());

        health -= damage;

        if (bloodEffect != null)
        {
            //bloodEffect.Stop();
            bloodEffect.Play();
        }

        ShakeScreen();

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    // resets hit animation shortly after being hit
    private IEnumerator ResetHitAnimation()
    {
        yield return new WaitForSeconds(0.03f);
        animator.SetBool("isHit", false);
    }

    // regenerates health over time
    private void RegenerateHealth()
    {
        if (canRegenerate)
        {
            health += regenAmount;
        }
    }

    // handles enemy death sequence
    private IEnumerator Die()
    {
        if (isDead) yield break;
        isDead = true;

        animator.SetBool("isAttacking", false);
        animator.SetBool("isDead", true);
        animator.Play("death", 0, 0f);

        if (deathEffect != null && !deathEffect.isPlaying)
        {
            deathEffect.Play();
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.linearVelocity = Vector3.zero;
        }

        waveSpawner spawner = FindObjectOfType<waveSpawner>();
        if (spawner != null)
        {
            spawner.RemoveEnemy(this.gameObject);
        }

        if (currencyHolder != null && health > -100)
        {
            currencyHolder.AddSkulls(skullsReward);
        }

        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }

    // triggers screen shake effect
    private void ShakeScreen()
    {
        Shake shakeScript = FindFirstObjectByType<Shake>();
        if (shakeScript != null)
        {
            StartCoroutine(shakeScript.Shaking());
        }
    }
}
