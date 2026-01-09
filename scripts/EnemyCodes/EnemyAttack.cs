//enemy chases and attacks player with cooldown and animations

using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    public Transform player;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public int attackDamage = 10;

    private Animator animator;
    private NavMeshAgent agent;
    private PlayerHealth playerHealth;
    private bool isAttacking = false;
    private float attackTimer = 0f;
    private float distanceToPlayer;
    private bool isNPC;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        isNPC = CompareTag("NPC");
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        if (player == null) return;

        distanceToPlayer = (transform.position - player.position).sqrMagnitude;

        if (animator.GetBool("isHit") || animator.GetBool("isDead"))
        {
            isAttacking = false;
            attackTimer = attackCooldown;
            animator.SetBool("isAttacking", false);
            return;
        }

        if (distanceToPlayer <= attackRange * attackRange)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero; //stop when close
        }
        else if (!isAttacking)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position); //chase player
        }

        if (!isAttacking && attackTimer <= 0f && distanceToPlayer <= attackRange * attackRange)
        {
            StartCoroutine(PerformAttack()); //start attack
        }

        if (attackTimer > 0f) attackTimer -= Time.deltaTime;
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;
        animator.SetBool("isAttacking", true);
        animator.Play("attack");

        yield return new WaitForSeconds(0.7f); //first hit
        if (IsPlayingAnimation("attack") && distanceToPlayer <= attackRange * attackRange)
        {
            playerHealth?.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(0.4f); //second hit
        if (IsPlayingAnimation("attack") && distanceToPlayer <= attackRange * attackRange)
        {
            playerHealth?.TakeDamage(attackDamage);
        }

        yield return new WaitForSeconds(0.3f); //end
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        if (distanceToPlayer <= attackRange * attackRange)
        {
            agent.velocity = Vector3.zero;
        }
    }

    private bool IsPlayingAnimation(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); //show attack range
    }
}
