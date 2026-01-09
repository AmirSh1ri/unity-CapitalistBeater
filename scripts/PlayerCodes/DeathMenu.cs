//handles death screen buttons

using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public void MainMenuLoader()
    {
        SceneManager.LoadScene(0); //load main menu
        Time.timeScale = 1; //reset time
    }

    public void QuitGameDeath()
    {
        Application.Quit(); //exit game
    }
}
