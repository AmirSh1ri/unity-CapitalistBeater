using UnityEngine;

public class BulletFollow : MonoBehaviour
{
    public float speed = 10f;
    public float trackingTime = 0.5f; 
    public float lifetime = 5f;
    public Vector3 rotSpeed = new Vector3(0f, 180f, 0f);

    private Transform player;
    private Vector3 direction;
    private float trackingTimer;

    void Start()
    {
        //find the player by tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        //set initial direction toward player
        if (player != null)
        {
            direction = (player.position - transform.position).normalized;
        }

        //initialize tracking timer and set bullet lifetime
        trackingTimer = trackingTime;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        //adjust direction while tracking is active
        if (trackingTimer > 0f && player != null)
        {
            Vector3 newDirection = (player.position - transform.position).normalized;
            direction = Vector3.Lerp(direction, newDirection, Time.deltaTime * 5f);
            trackingTimer -= Time.deltaTime;
        }

        //move bullet forward in current direction
        transform.position += direction * speed * Time.deltaTime;

        //rotate bullet for visual effect
        transform.Rotate(rotSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
