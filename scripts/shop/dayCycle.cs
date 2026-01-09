//switches skybox/fog between day and night
using TMPro;
using UnityEngine;
using System.Collections;

public class DayCycle : MonoBehaviour
{
    public Material skyboxMaterial;
    public bool isDay = true;
    private float transitionDuration = 10f;
    private bool TutorialShownOnce = false;
    private bool TutorialShownOnceNight = false;
        [Header("UI")]
    public GameObject nightTimer;
    public GameObject Tutorials;
    public TMP_Text nightTimerText;


    private void Start()
    {
        SetDay(); //start game by day
    }

    public void SetDay()
    {
        StartCoroutine(TransitionToDay());
        isDay = true;
        Debug.Log("setday1");
    }

    public void SetNight()
    {
        if (isDay)
        {
            StartCoroutine(TransitionToNight());
            isDay = false;
        }
    }

    private IEnumerator TransitionToDay()
    {
        Debug.Log("setday2");
        float elapsedTime = 0f;
        float startExposure = skyboxMaterial.GetFloat("_Exposure");
        float startFogDensity = RenderSettings.fogDensity;
        Color startFogColor = RenderSettings.fogColor;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;
            
            //fade to day values
            skyboxMaterial.SetFloat("_Exposure", Mathf.Lerp(startExposure, 8f, t));
            RenderSettings.fogDensity = Mathf.Lerp(startFogDensity, 0.01f, t);
            RenderSettings.fogColor = Color.Lerp(startFogColor, new Color32(19, 65, 161, 255), t);
            
            yield return null;
        }

        Debug.Log("setday3");
        skyboxMaterial.SetFloat("_Exposure", 8f);
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.fogColor = new Color32(19, 65, 161, 255);
    }

    private IEnumerator TransitionToNight()
    {
        if(!TutorialShownOnce){
            Tutorials.SetActive(true);
        TutorialShownOnce = true;
        }
        nightTimer.SetActive(true);
        float elapsedTime = 0f;
        float startExposure = skyboxMaterial.GetFloat("_Exposure");
        float startFogDensity = RenderSettings.fogDensity;
        Color startFogColor = RenderSettings.fogColor;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            int TimeTillNight = 10 - Mathf.FloorToInt(elapsedTime);
            nightTimerText.text = string.Format("00:{0:00}", TimeTillNight);
            float t = elapsedTime / transitionDuration;
            Mathf.CeilToInt(elapsedTime);

            //fade to night values
            skyboxMaterial.SetFloat("_Exposure", Mathf.Lerp(startExposure, 0.6f, t));
            RenderSettings.fogDensity = Mathf.Lerp(startFogDensity, 0.02f, t);
            RenderSettings.fogColor = Color.Lerp(startFogColor, new Color32(0, 0, 8, 255), t);

            yield return null;
        }
        nightTimer.SetActive(false);
        skyboxMaterial.SetFloat("_Exposure", 0.6f);
        RenderSettings.fogDensity = 0.02f;
        RenderSettings.fogColor = new Color32(0, 0, 8, 255);
    }
}
