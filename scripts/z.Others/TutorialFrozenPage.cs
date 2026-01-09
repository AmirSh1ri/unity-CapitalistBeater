using UnityEngine;

public class TimeController : MonoBehaviour
{
    private float savedTimeScale;
    public GameObject PauseBTN;
    void Start()
    {
        PauseBTN.SetActive(false);
        FreezeTime();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void FreezeTime()
    {
        savedTimeScale = Time.timeScale; //store current time scale
        Time.timeScale = 0f;
        Debug.Log("Time frozen");
    }

    public void UnfreezeTime()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseBTN.SetActive(true);
        Time.timeScale = savedTimeScale;
        Debug.Log("Time unfrozen");
        gameObject.SetActive(false);
    }
}