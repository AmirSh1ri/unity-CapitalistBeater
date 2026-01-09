//disables player control and hides weapon on death

using UnityEngine;

public class deathLogic : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public MouseLook mouseLook;
    public Attack playerAttack;
    public GameObject Weapon;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; //unlock cursor
        if (Weapon != null) Weapon.SetActive(false); //hide weapon
        if (playerMovement != null) playerMovement.enabled = false;
        if (mouseLook != null) mouseLook.enabled = false;
        if (playerAttack != null) playerAttack.enabled = false;
        if (playerHealth != null) playerHealth.enabled = false;
    }
}
