// plays random idle animation or movement animations based on distance to player

using UnityEngine;

public class PlayAnim : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    private bool isDone = true;
    private string[] animationNames = { "Idle", "lay", "lick", "stretch", "lick2" };

    // setup animator
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError("Animator not found!");
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < 11f && isDone)
        {
            Debug.Log("Played Once");
            PlayRandomAnimation();
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
        else if (distance >= 11f && distance <= 20f)
        {
            isDone = true;
            animator.SetBool("isWalking", true);    
            animator.Play("walk");
            animator.SetBool("isRunning", false);
        }
        else if (distance > 20f)
        {
            isDone = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
            animator.Play("run"); 
        }
    }

    // picks and plays a random idle animation
    void PlayRandomAnimation()
    {
        isDone = false;
        int index = Random.Range(0, animationNames.Length);
        string selectedAnim = animationNames[index];
        animator.Play(selectedAnim);
        Debug.Log("Playing: " + selectedAnim);
    }
}
