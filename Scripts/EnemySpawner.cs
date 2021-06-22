using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Enemy[] enemyPrefabs;

    [SerializeField]
    private Transform[] spawningPoints;

    [SerializeField]
    private float spawnRate = 10f;

    [SerializeField]
    private float initialSpawnDelay;

    [SerializeField]
    private int totalNumberToSpawn;

    [SerializeField]
    private int numberToSpawnEachTime = 1;

    private float spawnTimer;
    private int currentSpawnedNumber = 0;

    private void OnEnable()
    {
        spawnTimer = spawnRate - initialSpawnDelay;
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (ShouldSpawn())
        {
            SpawnEnemy();
            
        }
    }

    private bool ShouldSpawn()
    {
        if (totalNumberToSpawn > 0 && currentSpawnedNumber >= totalNumberToSpawn)
            return false;

            return spawnTimer >= spawnRate;
    }

    private void SpawnEnemy()
    {
        spawnTimer = 0;
        var availableSpawnPoints = spawningPoints.ToList();
        for (int i = 0; i < numberToSpawnEachTime; i++)
        { 
            Enemy enemyPrefab = PickRandomEnemyPrefab();
            if (totalNumberToSpawn > 0 && currentSpawnedNumber >= totalNumberToSpawn)
                break;

            if (enemyPrefab != null)
            {
                Transform spawnPoint = PickRandomSpawningPoint(availableSpawnPoints);
                if(availableSpawnPoints.Contains(spawnPoint))
                    availableSpawnPoints.Remove(spawnPoint);

                // var enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

                var enemy = enemyPrefab.Get<Enemy>(spawnPoint.position,spawnPoint.rotation);
                
                currentSpawnedNumber++;

            }
        }
    }

    private Transform PickRandomSpawningPoint(List<Transform> availableSpawnPoints)
    {
        if (availableSpawnPoints.Count == 0)
            return transform;

        if (availableSpawnPoints.Count == 1)
            return availableSpawnPoints[0];

        else
        {
            int index = UnityEngine.Random.Range(0, availableSpawnPoints.Count);
            return availableSpawnPoints[index];
        }
    }

    private Enemy PickRandomEnemyPrefab()
    {
        if (enemyPrefabs.Length == 0)
            return null;


        if (enemyPrefabs.Length == 1)
            return enemyPrefabs[0];

        else
        {
            int index = UnityEngine.Random.Range(0, enemyPrefabs.Length);
            return enemyPrefabs[index];
        }
    }
#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 1f);
        foreach(Transform spawn in spawningPoints)
        {
            Gizmos.DrawCube(spawn.position, Vector3.one);
        }
    }
    
#endif
}
