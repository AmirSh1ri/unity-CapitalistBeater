//enemy that moves toward player and throws a potion when in range

using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class RangeAttack : MonoBehaviour
{
    public Transform player;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public GameObject potion;
    public Transform spawnPot;

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

        distanceToPlayer = (transform.position - player.position).sqrMagnitude;

        //stop attack if hit or dead
        if (animator.GetBool("isHit") || animator.GetBool("isDead"))
        {
            isAttacking = false;
            attackTimer = attackCooldown;
            animator.SetBool("isAttacking", false);
            return;
        }

        //stop moving if in range
        if (distanceToPlayer <= attackRange * attackRange)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        //move toward player if not attacking
        else if (!isAttacking)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }

        //attack when ready and in range
        if (!isAttacking && attackTimer <= 0f && distanceToPlayer <= attackRange * attackRange)
        {
            StartCoroutine(PerformAttack());
        }

        //count down cooldown
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    //does the actual attack
    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        animator.SetBool("isAttacking", true);
        animator.Play("attack"); 

        yield return new WaitForSeconds(0.7f); 

        if (spawnPot != null) 
        {
            Instantiate(potion, spawnPot.position, spawnPot.rotation); 
            Debug.Log("Potion spawned at attack position!");
        }

        yield return new WaitForSeconds(0.4f); 
        yield return new WaitForSeconds(0.3f); 

        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    //checks if a specific animation is playing
    private bool IsPlayingAnimation(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    //shows attack range in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
