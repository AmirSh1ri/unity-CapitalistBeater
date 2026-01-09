//toggles spotlight on T key press

using UnityEngine;

public class ToggleLight : MonoBehaviour
{
    public GameObject spotLight;

    void Update()
    {
        //toggle light
        if (Input.GetKeyDown(KeyCode.T) && spotLight != null)
        {
            spotLight.SetActive(!spotLight.activeSelf);
        }
    }
}
