using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //lock cursor to center
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensivity; 
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensivity; 

        xRotation -= mouseY; //invert vertical input
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //limit vertical look angle

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //apply vertical rotation to camera

        playerBody.Rotate(Vector3.up * mouseX); 
    }
}
