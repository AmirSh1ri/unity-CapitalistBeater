// sets custom render queue for all particle objects in a specific layer at start to show over transparent obj which are 3000

using UnityEngine;

public class SetRenderQueueForLayer : MonoBehaviour
{
    public int targetLayer = 12; // "particles" layer
    public int renderQueueValue = 3100;

    void Start()
    {
        // Find all GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Only process objects on the target layer
            if (obj.layer == targetLayer)
            {
                // Access the ParticleSystemRenderer if it exists
                ParticleSystemRenderer psRenderer = obj.GetComponent<ParticleSystemRenderer>();
                if (psRenderer != null && psRenderer.material != null)
                {
                    psRenderer.material.renderQueue = renderQueueValue;
                }
            }
        }
    }
}
