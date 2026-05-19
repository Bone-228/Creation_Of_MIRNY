using UnityEngine;

public class enemy_spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;

    void Start()
    {
        SpawnEnemy();
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoint == null)
            return;

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}