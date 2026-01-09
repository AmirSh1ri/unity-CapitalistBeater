//controls the pause menu toggle and related UI/controls

using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject ESCMenu;
    public GameObject MainESCMenu;
    public GameObject OptionESCMenu;
    public PlayerMovement playerMovement;
    public PlayerHealth playerHealth;
    public MouseLook mouseLook;
    public Attack playerAttack;
    public AcidLevel acidLevel;

    private bool isPaused = false;

    //checks for escape key each frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    //toggles pause state and related systems
    public void TogglePause()
    {
        isPaused = !isPaused;
        if(isPaused){
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }

        ESCMenu.SetActive(isPaused);
        MainESCMenu.SetActive(isPaused);
        OptionESCMenu.SetActive(!isPaused);

        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;

        if (acidLevel != null) acidLevel.enabled = isPaused ? false : true;
        if (playerMovement != null) playerMovement.enabled = isPaused ? false : true;
        if (mouseLook != null) mouseLook.enabled = isPaused ? false : true;
        if (playerAttack != null) playerAttack.enabled = isPaused ? false : true;
        if (playerHealth != null) playerHealth.enabled = isPaused ? false : true;
    }
}
