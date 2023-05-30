using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnedParent;
    [SerializeField] private List<GameObject> enemies;

    private void Start()
    {
        SpawnEnemies(5);
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    private void SpawnEnemies (int count)
    {
        for (int i = 0; i < count; i++)
        {
            float randomXPosition = Random.Range(-10, 10);
            float randomYPosition = Random.Range(-10, 10);
            float randomZPosition = Random.Range(-10, 10);

            Vector3 randomPosition = new Vector3(randomXPosition, randomYPosition, randomZPosition);

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
