// calculates and displays average FPS every second

using UnityEngine;
using System.Collections;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    private int frameCount = 0;
    private float totalTime = 0f;
    private float updateInterval = 1f;

    [Header("UI")]
    public TextMeshProUGUI fpsDisplayText;

    // starts the FPS calculation coroutine
    void Start()
    {
        StartCoroutine(CalculateFPS());
    }

    void Update()
    {
        frameCount++;
        totalTime += Time.deltaTime;
    }

    // calculates and displays average FPS every interval
    private IEnumerator CalculateFPS()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateInterval);

            float averageFPS = frameCount / totalTime;
            int fpsText = (int)averageFPS;

            if (fpsDisplayText != null){
                fpsDisplayText.text = "" + fpsText;
            }

            frameCount = 0;
            totalTime = 0f;
        }
    }
}
