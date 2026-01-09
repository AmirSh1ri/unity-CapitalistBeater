// handles starting the game, showing loading UI, and quitting the app

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public TMP_Text progressText;

    // starts loading the selected scene
    public void PlayGame(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    // loads the scene asynchronously and updates the loading UI
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);      

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = progress * 100 + "%";
            yield return null;
        }
    }

    // quits the game (does not work in editor)
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("2");
    }
}
