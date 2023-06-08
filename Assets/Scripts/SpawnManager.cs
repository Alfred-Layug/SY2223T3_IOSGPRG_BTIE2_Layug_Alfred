using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnedParent;
    [SerializeField] public List<GameObject> enemies;

    private float spawnEnemyTimer;
    private float currentSpawnEnemyTimer;

    public bool _playerIsAlive;

    public void Start()
    {
        _playerIsAlive = true;

        SpawnEnemies(1);

        spawnEnemyTimer = Random.Range(2.0f, 3.0f);
        currentSpawnEnemyTimer = spawnEnemyTimer;
    }

    private void Update()
    {
        if (_playerIsAlive == true)
        {
            if (currentSpawnEnemyTimer > 0)
            {
                currentSpawnEnemyTimer -= Time.deltaTime;
            }
            else
            {
                SpawnEnemies(1);
                spawnEnemyTimer = Random.Range(2.0f, 3.0f);
                currentSpawnEnemyTimer = spawnEnemyTimer;
            }
        }
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public void SpawnEnemies (int count)
    {
        Debug.Log("Spawned!");
        for (int i = 0; i < count; i++)
        {
            float randomXPosition = Random.Range(-1.0f, 0.5f);
            float randomYPosition = Random.Range(15, 20);

            Vector3 randomPosition = new Vector3(randomXPosition, randomYPosition, 0);

            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            enemy.transform.parent = spawnedParent;

            enemies.Add(enemy);

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript._attack = 10;
            enemyScript._defense = 10;
            enemyScript._health = Random.Range(5, 10);
        }
    }
}
