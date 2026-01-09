using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class waveSpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public List<Enemies> enemies = new List<Enemies>();
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    private List<GameObject> activeEnemies = new List<GameObject>();
    public GameObject bossEnemyPrefab;
    
    [Header("Wave Settings")]
    public int currWave;
    public int waveValue;
    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;
    private bool waveStarted = false;
    private bool fifteenSecTimer = false;
    private bool postWaveTimerRunning = true;
    private int numEnemiesToAdd;
    public GameObject TutorialsNight;
    private bool TutorialShownOnceNight = false;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    [Header("UI Elements")]
    public TextMeshProUGUI enemyCountText;
    public TextMeshProUGUI untilDayTime;
    public GameObject dayTimerUI;

    [Header("External References")]
    private DayCycle dayCycle;
    private ShopMoveAway shopMoveAway;

    private bool shopMovedAtNight = false;
    private bool shopMovedAtDay = false;

    // Unity Methods

    void Start()
    {
        dayCycle = FindObjectOfType<DayCycle>();
        shopMoveAway = FindObjectOfType<ShopMoveAway>();
        UpdateEnemyCountUI();
    }
    void Update()
{
    // Check if both "K" and "I" keys are pressed together
    if (Input.GetKey(KeyCode.K) && Input.GetKey(KeyCode.I))
    {
        StartCoroutine(TransitionToDay());
    }
}
    void FixedUpdate()
    {
        if (dayCycle != null && !waveStarted)
        {
            if (!dayCycle.isDay)
            {
                if( currWave == 9){
                    //boss method 
                    //return making it so you 
                    return;
                }
                if (currWave == 5)
                {
                    StartCoroutine(BossWave());
                    dayCycle.isDay = false;
                    waveStarted = true;
                    Debug.Log("BossWave");

                    if (shopMoveAway != null && !shopMovedAtNight)
                    {
                        StartCoroutine(shopMoveAway.MoveSequence());
                        shopMovedAtNight = true;
                        shopMovedAtDay = false;
                    }

                    return;
                }else{

                    StartCoroutine(StartWaveAfterDelay());
                dayCycle.isDay = false;
                waveStarted = true;
                Debug.Log("Wave Starts after 10 sec");

                if (shopMoveAway != null && !shopMovedAtNight)
                {
                    StartCoroutine(shopMoveAway.MoveSequence());
                    shopMovedAtNight = true;
                    shopMovedAtDay = false;
                }

                }

                
            }
        }

        if (enemiesToSpawn.Count == 0 && activeEnemies.Count == 0 && !dayCycle.isDay && fifteenSecTimer && postWaveTimerRunning)
        {
            Debug.Log("Wave Over???");
            StartCoroutine(Timer());
            postWaveTimerRunning = false;
        }

        if (spawnTimer <= 0)
        {
            if (enemiesToSpawn.Count > 0)
            {
                int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
                int randomEnemyIndex = Random.Range(0, enemiesToSpawn.Count);
                GameObject newEnemy = Instantiate(enemiesToSpawn[randomEnemyIndex], spawnPoints[randomSpawnIndex].position, Quaternion.identity);
                activeEnemies.Add(newEnemy);
                enemiesToSpawn.RemoveAt(randomEnemyIndex);
                spawnTimer = spawnInterval;
                UpdateEnemyCountUI();
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
    }

    // Wave Management
        private IEnumerator BossWave()
    {

        dayTimerUI.SetActive(true);
        float timer = 120f;
        int bossesSpawned = 0;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            untilDayTime.text = string.Format("00:{0:00}", Mathf.CeilToInt(timer));

            if (bossesSpawned < 5)
            {
                int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
                GameObject newBoss = Instantiate(bossEnemyPrefab, spawnPoints[randomSpawnIndex].position, Quaternion.identity);
                activeEnemies.Add(newBoss);
                bossesSpawned++;
            }

            yield return null;
        }
        ApplyDamageToAllEnemies(500);
        Debug.Log("Boss wave ended, transitioning to day...");
        dayTimerUI.SetActive(false);
        StartCoroutine(TransitionToDay());
    }

    private IEnumerator StartWaveAfterDelay()
    {
        Debug.Log("Waiting 10 seconds before starting wave...");
        yield return new WaitForSeconds(10f);

        GenerateWave();
        yield return new WaitForSeconds(5f);
        fifteenSecTimer = true;
        Debug.Log("15 true");
    }

    public void GenerateWave()
    {
        waveValue = currWave * currWave * 5 + 10;
        Debug.Log(waveValue);
        GenerateEnemies();

        spawnInterval = (enemiesToSpawn.Count > 0) ? waveDuration / enemiesToSpawn.Count : 1f;
        waveTimer = waveDuration;
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();
        int safetyCounter = 1000;

        while (waveValue > 0 && safetyCounter > 0)
        {
            int randEnemyId = Random.Range(0, enemies.Count);
            int randEnemyCost = enemies[randEnemyId].cost;

            if (waveValue - randEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                waveValue -= randEnemyCost;
            }
            safetyCounter--;
        }

        if (safetyCounter <= 0)
        {
            Debug.LogError("GenerateEnemies() reached safety limit! Check enemy costs.");
        }
        enemiesToSpawn = new List<GameObject>(generatedEnemies);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
            UpdateEnemyCountUI();
        }
    }

    // Timer and Transition

    private IEnumerator Timer()
    {
        if(!TutorialShownOnceNight){
            
            TutorialsNight.SetActive(true);
            TutorialShownOnceNight = true;
            
            }
        dayTimerUI.SetActive(true);
        float timer = 30;

            if (enemiesToSpawn.Count == 0)
            {
                numEnemiesToAdd = 99;
                RefillEnemiesToSpawn();
            }

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            untilDayTime.text = string.Format("00:{0:00}", timer);;



            if (enemiesToSpawn.Count > 0)
            {
                int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
                int randomEnemyIndex = Random.Range(0, enemiesToSpawn.Count);
                GameObject newEnemy = Instantiate(enemiesToSpawn[randomEnemyIndex], spawnPoints[randomSpawnIndex].position, Quaternion.identity);
                activeEnemies.Add(newEnemy);
                enemiesToSpawn.RemoveAt(randomEnemyIndex);
                UpdateEnemyCountUI();
            }

            yield return null;
        }

        Debug.Log("30 seconds passed, transitioning to day...");
        dayTimerUI.SetActive(false);
        StartCoroutine(TransitionToDay());
        if (shopMoveAway != null && !shopMovedAtNight)
                {
                    StartCoroutine(shopMoveAway.ReverseSequence());
                    shopMovedAtNight = false;
                    shopMovedAtDay = true;
                }
    }

    private IEnumerator TransitionToDay()
    {
        dayCycle.isDay = true;
        fifteenSecTimer = false;
        Debug.Log("Transitioning to day in 5 seconds...");
        ApplyDamageToAllEnemies(500);
        enemiesToSpawn.Clear();
        activeEnemies.Clear();

        yield return new WaitForSeconds(2f);

        dayCycle.SetDay();
        waveStarted = false;
        currWave++;
        postWaveTimerRunning = true;
        Debug.Log("Daytime! Wave counter increased.");

        if (shopMoveAway != null && !shopMovedAtDay)
        {
            StartCoroutine(shopMoveAway.ReverseSequence());
            shopMovedAtDay = true;
            shopMovedAtNight = false;
        }
    }

    private void ApplyDamageToAllEnemies(int damage)
    {
        // Make a copy of the enemies because of a bug that didn't let half 
        // The enemies get destroyed because the order got update after each removal
        List<GameObject> enemiesCopy = new List<GameObject>(activeEnemies); 

        foreach (GameObject enemyACT in enemiesCopy)
        {
            if (enemyACT != null)
            {
                Enemy enemyScript = enemyACT.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damage);
                }
                else
                {
                    Debug.LogWarning("Enemy GameObject missing Enemy script: " + enemyACT.name);
                }
            }
        }
    }


    private void RefillEnemiesToSpawn()
    {
        if (activeEnemies.Count < 100)
        {
            for (int i = 0; i < numEnemiesToAdd; i++)
            {
                int randomEnemyIndex = Random.Range(0, enemies.Count);
                enemiesToSpawn.Add(enemies[randomEnemyIndex].enemyPrefab);
            }
            numEnemiesToAdd = 1;
        }
    }

    private void UpdateEnemyCountUI()
    {
        if (enemyCountText != null)
        {
            enemyCountText.text = "" + activeEnemies.Count;
        }
    }
}

[System.Serializable]
public class Enemies
{
    public GameObject enemyPrefab;
    public int cost;
}
