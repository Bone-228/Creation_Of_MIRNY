using System.Collections.Generic;
using UnityEngine;

public class enemy_sector_checker : MonoBehaviour
{
    [Header("Gate Settings")]
    public gateControll[] gates;

    [Header("All Gates To Reset")]
    public gateControll[] allGatesToClose;

    private List<enemy_health> enemies = new List<enemy_health>();
    private bool gateOpened = false;

    public int aliveEnemies;

    private enemy_spawner[] spawners;

    void Awake()
    {
        spawners = GetComponentsInChildren<enemy_spawner>();
    }

    void OnEnable()
    {
        faze_handler.OnFazeChanged += ResetSector;
    }

    void OnDisable()
    {
        faze_handler.OnFazeChanged -= ResetSector;
    }

    void Update()
    {
        CleanAndCountEnemies();
    }

    void OnTriggerEnter(Collider other)
    {
        enemy_health enemy = other.GetComponentInParent<enemy_health>();

        if (enemy != null && !enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    void OnTriggerExit(Collider other)
    {
        enemy_health enemy = other.GetComponentInParent<enemy_health>();

        if (enemy != null)
        {
            enemies.Remove(enemy);
        }
    }

    public void EnemyDied(enemy_health enemy)
    {
        if (enemy == null) return;

        enemies.Remove(enemy);
    }

    void CleanAndCountEnemies()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null || enemies[i].isDead)
            {
                enemies.RemoveAt(i);
            }
        }

        aliveEnemies = enemies.Count;

        if (aliveEnemies == 0 && !gateOpened)
        {
            OpenRandomGate();
            gateOpened = true;
        }
    }

    void OpenRandomGate()
    {
        if (gates == null || gates.Length == 0)
            return;

        List<gateControll> closedGates = new List<gateControll>();

        foreach (gateControll gate in gates)
        {
            if (gate == null || gate.gateAnimator == null)
                continue;

            if (!gate.gateAnimator.GetBool("Open"))
            {
                closedGates.Add(gate);
            }
        }

        if (closedGates.Count == 0)
            return;

        int randomIndex = Random.Range(0, closedGates.Count);

        closedGates[randomIndex].gateAnimator.SetBool("Open", true);
    }

    void ResetSector(int newFaze)
    {
        DeleteAllEnemies();
        RespawnEnemies();
        CloseAllGates();

        gateOpened = false;
    }

    void DeleteAllEnemies()
    {
        foreach (enemy_health enemy in enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        enemies.Clear();
        aliveEnemies = 0;
    }

    void RespawnEnemies()
    {
        if (spawners == null || spawners.Length == 0)
            return;

        foreach (enemy_spawner spawner in spawners)
        {
            if (spawner != null)
            {
                spawner.SpawnEnemy();
            }
        }
    }

    void CloseAllGates()
    {
        if (allGatesToClose == null)
            return;

        foreach (gateControll gate in allGatesToClose)
        {
            if (gate != null && gate.gateAnimator != null)
            {
                gate.gateAnimator.SetBool("Open", false);
            }
        }
    }
}