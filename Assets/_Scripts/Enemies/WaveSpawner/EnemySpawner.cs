using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPoints = new List<Transform>();
    [SerializeField] List<WaveOveride> enemyWaveOverride = new List<WaveOveride>();
    WaveOveride currentWaveOveride;
    [SerializeField] float spawnRate;
    float nextWave = 5f;
    float timer;
    int currentOverride = 0;
    public int waveIndex = 0;

    private void Start()
    {        
        currentWaveOveride = enemyWaveOverride[0];
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > nextWave)
        {
            if (enemyWaveOverride.Count > currentOverride)
            {
                if (waveIndex == enemyWaveOverride[currentOverride].overrideWaveIndex)
                {
                    currentWaveOveride = enemyWaveOverride[currentOverride];
                    currentOverride++;
                }
            }
            timer = 0;
            nextWave += 2;
            waveIndex++;
            if(spawnRate >= 4)
            {
                spawnRate -= -0.5f;
            }            
        }
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds (5); 
        while (true)
        {
            int odds = 1;
            if(waveIndex > 3)
            {
                odds = Random.Range(1, currentWaveOveride.overrideEnemyPerSpawn + 1);
            }
            for (int i = 0; i < odds; i++)
            {
                GameObject enemy = Instantiate(currentWaveOveride.waveEnemies[Random.Range(0, currentWaveOveride.waveEnemies.Length)]);
                enemy.GetComponent<NavMeshAgent>().Warp(spawnPoints[Random.Range(0, spawnPoints.Count)].position);
            }           
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
