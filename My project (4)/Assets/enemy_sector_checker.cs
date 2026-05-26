
using System.Collections.Generic;
using UnityEngine;

public class enemy_sector_checker : MonoBehaviour
{
    [Header("Gate Settings")]
    public gateControll[] gates;

    [Header("All Gates In Entire Level")]
    public gateControll[] allGatesToClose;

    private List<enemy_health> enemies =
        new List<enemy_health>();

    private bool gateOpened = false;

    // Prevents gates from opening during resets
    private bool sectorReady = true;

    public int aliveEnemies;

    private enemy_spawner[] spawners;

    // ───────────────────────── INIT ─────────────────────────

    private void Start()
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

    // ───────────────────────── CLEANUP ─────────────────────────

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

        // Gates can ONLY open when sector is ready
        if (sectorReady &&
            aliveEnemies == 0 &&
            !gateOpened)
        {
            OpenRandomGate();

            gateOpened = true;
        }
    }

    // ───────────────────────── OPEN RANDOM CLOSED GATE ─────────────────────────

    void OpenRandomGate()
    {
        if (gates == null || gates.Length == 0)
            return;

        List<gateControll> candidates =
            new List<gateControll>();

        foreach (gateControll gate in gates)
        {
            if (gate != null && gate.gateAnimator != null)
            {
                int state =
                    gate.gateAnimator.GetInteger("State");

                // ONLY CLOSED GATES
                if (state == 0)
                {
                    candidates.Add(gate);
                }
            }
        }

        // No closed gates available
        if (candidates.Count == 0)
        {
            Debug.Log("No closed gates available.");
            return;
        }

        int randomIndex =
            Random.Range(0, candidates.Count);

        gateControll selectedGate =
            candidates[randomIndex];

        selectedGate.OpenGate();

        Debug.Log(
            "Opened gate: " +
            selectedGate.gameObject.name
        );
    }

    // ───────────────────────── FAZE RESET ─────────────────────────

    void ResetSector(int newFaze)
    {
        // Prevent gates from opening during reset
        sectorReady = false;

        gateOpened = false;

        DeleteAllEnemies();

        CloseAllGates();

        RespawnEnemies();

        // Sector works normally again
        sectorReady = true;
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

    // ───────────────────────── RESPAWN ─────────────────────────

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

        Debug.Log("All gates closed.");
    }
}

