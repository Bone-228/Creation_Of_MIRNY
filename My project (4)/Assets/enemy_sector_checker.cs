using System.Collections.Generic;
using UnityEngine;

public class enemy_sector_checker : MonoBehaviour
{
    [Header("Gate Settings")]
    public gateControll[] gates;

    [Header("All Gates In Entire Level (assign manually in Inspector)")]
    public gateControll[] allGatesToClose;

    private List<enemy_health> enemies =
        new List<enemy_health>();

    private bool gateOpened = false;

    public int aliveEnemies;

    private enemy_spawner[] spawners;

    // ───────────────────────── INIT ─────────────────────────
    void Start()
    {
        allGatesToClose = gates;
    }
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

    // ───────────────────────── UPDATE ─────────────────────────

    void Update()
    {
        CleanAndCountEnemies();
    }

    // ───────────────────────── ENEMY TRACKING ─────────────────────────

    void OnTriggerEnter(Collider other)
    {
        enemy_health enemy =
            other.GetComponentInParent<enemy_health>();

        if (enemy != null && !enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    void OnTriggerExit(Collider other)
    {
        enemy_health enemy =
            other.GetComponentInParent<enemy_health>();

        if (enemy != null)
        {
            enemies.Remove(enemy);
        }
    }

    public void EnemyDied(enemy_health enemy)
    {
        if (enemy == null)
            return;

        enemies.Remove(enemy);
    }

    // ───────────────────────── ENEMY CLEANUP ─────────────────────────

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

    // ───────────────────────── OPEN GATE ─────────────────────────

    void OpenRandomGate()
    {
        if (gates == null || gates.Length == 0)
            return;

        List<gateControll> candidates =
            new List<gateControll>();

        foreach (gateControll gate in gates)
        {
            if (gate != null)
            {
                candidates.Add(gate);
            }
        }

        if (candidates.Count == 0)
            return;

        int randomIndex =
            Random.Range(0, candidates.Count);

        gateControll selectedGate =
            candidates[randomIndex];

        selectedGate.OpenGate();
    }

    // ───────────────────────── FAZE RESET ─────────────────────────

    void ResetSector(int newFaze)
    {
        DeleteAllEnemies();
        RespawnEnemies();
        CloseAllGates();

        gateOpened = false;
    }

    // ───────────────────────── DELETE ENEMIES ─────────────────────────

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

    // ───────────────────────── CLOSE ALL GATES ─────────────────────────

    void CloseAllGates()
    {
        if (allGatesToClose == null)
            return;

        foreach (gateControll gate in allGatesToClose)
        {
            if (gate != null)
            {
                gate.CloseGate();
            }
        }
    }
}