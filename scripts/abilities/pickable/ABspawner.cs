///this script randomly spawns prefabs at predefined spawn points every 30 seconds
///any previously spawned objects are cleared before new ones are spawned

using System.Collections;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] prefabs;         //array of possible prefabs to spawn
    public Transform[] spawnPoints;      //spawn locations in the scene

    private GameObject[] currentSpawned;

    void Start()
    {
        currentSpawned = new GameObject[spawnPoints.Length];
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnAll();                             //spawn a prefab at each point
            yield return new WaitForSeconds(30f);
            ClearAll();                             //remove old prefabs
        }
    }

    private void SpawnAll()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform point = spawnPoints[i];

            //choose a random prefab from the list
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

            //spawn the prefab at the current spawn point
            GameObject spawned = Instantiate(prefab, point.position, point.rotation);

            //store the reference so it can be cleared later
            currentSpawned[i] = spawned;
        }
    }

    private void ClearAll()
    {
        for (int i = 0; i < currentSpawned.Length; i++)
        {
            //destroy the current object if it exists
            if (currentSpawned[i] != null)
            {
                Destroy(currentSpawned[i]);
                currentSpawned[i] = null;
            }
        }
    }
}
