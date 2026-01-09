//handles enemy navigation toward the player with randomized speed

using UnityEngine;
using UnityEngine.AI;

public class NavmeshCode : MonoBehaviour
{
    public Transform player;
    public float minSpeed = 25f;
    public float maxSpeed = 35f;
    public float rePathDistance = 1.5f; //minimum distance before updating path

    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 currentTarget;

    //initializes navmesh, sets random speed, targets player
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = Random.Range(minSpeed, maxSpeed);
        agent.stoppingDistance = 1.0f;

        if (player != null)
        {
            currentTarget = player.position;
            agent.SetDestination(currentTarget);
        }
    }

    //updates enemy movement if alive and player moved enough
    void Update()
    {
        if (IsDead())
        {
            if (agent.enabled) agent.enabled = false;
            return;
        }

        if (player != null)
        {
            if (!agent.enabled) agent.enabled = true;

            float distanceFromTarget = Vector3.Distance(currentTarget, player.position);

            if (distanceFromTarget > rePathDistance)
            {
                currentTarget = player.position;
                agent.SetDestination(currentTarget);
            }
        }
    }

    //checks if npc is dead via animator
    bool IsDead()
    {
        return animator != null && animator.GetBool("isDead");
    }
}
