using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawner Options")]
    [SerializeField] private List<Wave> waves;

    [Header("Display")]
    [SerializeField] private TextMeshProUGUI waveNumberText;
    [SerializeField] private TextMeshProUGUI enemiesCountText;
    [SerializeField] private TextMeshProUGUI timeToNextWave;

    [Header("SpawnArea")]
    [SerializeField] private SphereCollider sphereCollider;

    [SerializeField] CardSelector cardSelector;

    public event System.Action OnGameCompleted;
    public event System.Action OnBeforeSave;


    bool cardSelecting = false;
    //Components
    Wave currentWave;

    //Booleans
    bool waveStarted;
    bool isGameCompleted;

    //Variables
    int remainingEnemyCount;
    int maxWaveCount;
    int currentWaveNumber;

    public void StartWaves()
    {
        foreach (var wave in waves)
        {
            maxWaveCount++;
        }
        StartCoroutine(NextWave());
    }

    private void Update()
    {
        if (isGameCompleted) return;
        if (!waveStarted && currentWaveNumber < maxWaveCount)
        {
            StartCoroutine(NextWave());
        }

        if (currentWaveNumber >= waves.Count && remainingEnemyCount <= 0 && !isGameCompleted)
        {
            isGameCompleted = true;
            OnBeforeSave.Invoke();
            StartCoroutine(OnGameCompletedCouroutine());
        }
    }

    IEnumerator OnGameCompletedCouroutine()
    {
        yield return new WaitForSeconds(2);
        OnGameCompleted.Invoke();

    }
    void OnEnemyDeath()
    {
        remainingEnemyCount--;
        UpdateDisplay();
        if (remainingEnemyCount <= 0)
        {
            waveStarted = false;
        }
    }

    IEnumerator NextWave()
    {
        waveStarted = true;
        yield return cardSelector.OpenCardSelection();
        yield return new WaitWhile(CheckCardSelection);
        if (currentWave == null)
        {
            SelectWave();
        }
        else
        {
            float counter = currentWave.timeBetweenWave;
            while (counter > 0)
            {
                counter--;
                timeToNextWave.text = string.Format("{0} sec", counter);
                yield return new WaitForSeconds(1f);
            }
            SelectWave();
        }
        UpdateDisplay();
    }

    void SelectWave()
    {

        if (currentWave == null)
        {
            currentWave = waves[0];
            currentWaveNumber = 1;
        }
        else
        {
            int index = waves.IndexOf(currentWave);
            currentWave = waves[index + 1];
            currentWaveNumber++;
        }

        foreach (var enemy in currentWave.enemyType)
        {
            remainingEnemyCount += enemy.enemyCount;
        }

        foreach (var item in currentWave.enemyType)
        {
            StartCoroutine(SpawnEnemy(item));
        }
        UpdateDisplay();
    }
    IEnumerator SpawnEnemy(EnemyType enemyType)
    {
        int enemyCount = enemyType.enemyCount;
        int enemyIndex = 0;
        while (enemyCount > 0)
        {
            Enemy newEnemy = Instantiate(enemyType.enemy, RandomPointAroundCircle(), Quaternion.identity);
            newEnemy.gameObject.name = string.Format("{0}_{1}", newEnemy.gameObject.name, enemyIndex);
            newEnemy.OnDeath += OnEnemyDeath;
            enemyCount--;
            enemyIndex++;
            yield return new WaitForSeconds(enemyType.timeBetweenSpawns);
        }
    }

    private Vector3 RandomPointAroundCircle()
    {
        Vector3 spawnDir = Random.insideUnitCircle.normalized * sphereCollider.radius;

        Vector3 selectedPos = new Vector3(spawnDir.x, 0.05f, spawnDir.y);

        return selectedPos;
    }

    public void UpdateDisplay()
    {
        waveNumberText.text = "Wave  " + currentWaveNumber;
        enemiesCountText.text = remainingEnemyCount.ToString();
        timeToNextWave.text = string.Format("{0}", currentWave.timeBetweenWave);
    }

    private bool CheckCardSelection()
    {
        return cardSelecting;
    }

    public void CardSelecting(bool state)
    {
        cardSelecting = state;
    }
}

[System.Serializable]
public class Wave
{
    public EnemyType[] enemyType;
    public float timeBetweenWave;
}

[System.Serializable]
public class EnemyType
{
    public Enemy enemy;
    public int enemyCount;
    public float timeBetweenSpawns;
}