//this script randomly triggers a particle system at intervals between minDelay and maxDelay

using UnityEngine;

public class RandomParticleTrigger : MonoBehaviour
{
    public ParticleSystem breath; 
    public float minDelay = 5f;
    public float maxDelay = 15f;

    void Start()
    {
        StartCoroutine(PlayParticles());
    }

    private System.Collections.IEnumerator PlayParticles()
    {
        while (true)
        {
            //wait for a random time between minDelay and maxDelay
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            if (breath != null)
                breath.Play();
        }
    }
}
