using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

public class GraphicsSettingsManager : MonoBehaviour
{
    [Header("Post-Processing")]
    public Volume globalVolume;
    private Bloom bloom;
    private MotionBlur dof; //dof is actually motion blur now
    private FilmGrain filmGrain;

    [Header("UI Controls")]
    public Slider volumeSlider;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown graphicsDropdown;
    public Toggle fpsShowToggle;
    public Slider fpsSlider;
    public TextMeshProUGUI fpsValueText;
    public GameObject fps;
    private bool isFPSVisible;

    public Toggle[] antiAliasingToggles;

    void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = 1.0f;
            SetVolume(1.0f); //initialize volume
        }

        Screen.fullScreen = true;
        globalVolume.profile.TryGet(out bloom);
        globalVolume.profile.TryGet(out dof); //get motion blur
        globalVolume.profile.TryGet(out filmGrain);

        resolutionDropdown.onValueChanged.AddListener(SetResolutionFromDropdown);
        graphicsDropdown.onValueChanged.AddListener(SetGraphicsFromDropdown);
        volumeSlider.onValueChanged.AddListener(SetVolume);
        fpsSlider.onValueChanged.AddListener(delegate { SetFPSLimit((int)fpsSlider.value); });

        fpsValueText.text = fpsSlider.value.ToString("None"); //set initial fps value text
    }

    //sets low quality settings
    public void SetGraphicsLow()
    {
        QualitySettings.SetQualityLevel(0);

        if (bloom != null) bloom.intensity.value = 0f;
        if (dof != null) dof.intensity.value = 0f;
        if (filmGrain != null) filmGrain.intensity.value = 0f;
    }

    //sets medium quality settings
    public void SetGraphicsMedium()
    {
        QualitySettings.SetQualityLevel(2);

        if (bloom != null) bloom.intensity.value = 25.6f;
        if (dof != null) dof.intensity.value = 0f;
        if (filmGrain != null) filmGrain.intensity.value = 0.43f;
    }

    //sets high quality settings
    public void SetGraphicsHigh()
    {
        QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1);

        if (bloom != null) bloom.intensity.value = 25.6f;
        if (dof != null) dof.intensity.value = 0.4f;
        if (filmGrain != null) filmGrain.intensity.value = 0.43f;
    }

    //sets quality based on dropdown selection
    public void SetGraphicsFromDropdown(int index)
    {
        string selected = graphicsDropdown.options[index].text.ToLower();

        switch (selected)
        {
            case "low":
                SetGraphicsLow();
                break;
            case "medium":
                SetGraphicsMedium();
                break;
            case "high":
                SetGraphicsHigh();
                break;
        }
    }

    //sets screen resolution from dropdown
    public void SetResolutionFromDropdown(int index)
    {
        string resText = resolutionDropdown.options[index].text;
        string[] dimensions = resText.Split('x');
        if (dimensions.Length == 2 &&
            int.TryParse(dimensions[0].Trim(), out int width) &&
            int.TryParse(dimensions[1].Trim(), out int height))
        {
            Screen.SetResolution(width, height, Screen.fullScreen);
        }
    }

    //toggles fps ui on or off
    public void ToggleFPS()
    {
        if (fpsShowToggle == null || fps == null)
        {
            return;
        }

        isFPSVisible = fpsShowToggle.isOn;
        fps.SetActive(isFPSVisible);
    }

    //sets master volume level
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    //sets target frame rate limit
    public void SetFPSLimit(int targetFPS)
    {
        Application.targetFrameRate = targetFPS;
        fpsValueText.text = targetFPS.ToString();
    }
}
