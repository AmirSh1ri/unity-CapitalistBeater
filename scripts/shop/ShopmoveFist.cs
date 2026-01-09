// the shop rotation code but for fist because of weird model
using UnityEngine;

public class ShopMoveFist : MonoBehaviour
{
    public float floatSpeed = 0.5f; 
    public float floatHeight = 0.2f; 

    private float startY;

    void Start()
    {
        startY = transform.position.y;  
    }

    void Update()
    {
         
        float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
