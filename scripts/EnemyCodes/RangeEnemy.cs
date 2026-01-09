//Enemy that shoots a projectile at the player when in range and not on cooldown

using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class RangeEnemy : MonoBehaviour
{
    public Transform player;
    public GameObject rangeOBJ; 
    public Transform firePoint; 
    public float attackRange = 10f;
    public float attackCooldown = 2f;
    public int attackDamage = 10;
    public float projectileSpeed = 10f;

    private Animator animator;
    private NavMeshAgent agent;
    private PlayerHealth playerHealth;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float distanceToPlayer;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null) return;

        //calculate squared distance to player
        distanceToPlayer = (transform.position - player.position).sqrMagnitude;

        //cancel attacking if hit or dead
        if (animator.GetBool("isHit") || animator.GetBool("isDead"))
        {
            isAttacking = false;
            attackTimer = attackCooldown;
            animator.SetBool("isAttacking", false);
            return;
        }

        //stop moving if within attack range
        if (distanceToPlayer <= attackRange * attackRange)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else if (!isAttacking)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

        //begin attack if ready and in range
        if (!isAttacking && attackTimer <= 0f && distanceToPlayer <= attackRange * attackRange)
        {
            StartCoroutine(PerformRangedAttack());
        }

        //reduce cooldown
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    //handles animation timing and firing projectile
    private IEnumerator PerformRangedAttack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        animator.SetBool("isAttacking", true);
        animator.Play("attack");

        yield return new WaitForSeconds(0.5f); //wait before firing

        if (IsPlayingAnimation("attack"))
        {
            SpawnProjectile();
        }

        yield return new WaitForSeconds(0.5f); //wait before resetting

        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    //spawns a projectile and sends it toward the player
    private void SpawnProjectile()
    {
        if (rangeOBJ != null && firePoint != null)
        {
            GameObject projectile = Instantiate(rangeOBJ, firePoint.position, Quaternion.identity);
            //Rigidbody rb = projectile.GetComponent<Rigidbody>();
            //if (rb != null)
            //{
            //    Vector3 direction = (player.position - firePoint.position).normalized;
            //    rb.linearVelocity = direction * projectileSpeed;
            //}
        }
    }

    //check if a specific animation is currently playing
    private bool IsPlayingAnimation(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    //draw attack range gizmo in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
