//rotates and floats the buy/pickable stuff slowly in place

using UnityEngine;

public class ShopMove : MonoBehaviour
{
    public float rotationSpeed = 20f;  
    public float floatSpeed = 0.5f;    
    public float floatHeight = 0.2f;   

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;  //save starting pos
    }

    void Update()
    {
        //rotate around y 
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + (rotationSpeed * Time.deltaTime), 0);

        //float up/down
        float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
