using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class faze_handler : MonoBehaviour
{
    public static Action<int> OnFazeChanged;

    [Header("Faze Borders")]
    public int faze2Border = 90;

    public int faze3Border = 180;

    public int backToSafe = 200;

    [Header("References")]
    public miriumPlayerCollector playerCollector;

    public Transform player;

    [Header("After Battle")]
    public AfterBattleUI afterBattleUI;

    [Header("Teleport Destinations")]
    public Transform[] randomDestinations;

    [Header("UI")]
    public TextMeshProUGUI fazeText;

    public TextMeshProUGUI playerMiriumText;

    public TextMeshProUGUI nextFazeBorderText;

    public TextMeshProUGUI mirium_collector_player_text;

    [Header("Mirium Progress Bar")]
    public Image miriumFillImage;

    [Header("FAZE 2 DECORATIONS")]
    public GameObject faze2Decorations;

    [Header("FAZE 3 DECORATIONS")]
    public GameObject faze3Decorations;

    private int currentFaze = 1;

    private bool runFinished = false;

    void Update()
    {
        if (playerCollector == null || runFinished)
            return;

        int mirium =
            playerCollector.collectedMirium;

        // ───────────────────────── FAZE CHECKS ─────────────────────────

        if (currentFaze == 1 &&
            mirium >= faze2Border)
        {
            ActivateFaze2();
        }
        else if (currentFaze == 2 &&
                 mirium >= faze3Border)
        {
            ActivateFaze3();
        }

        // ───────────────────────── RUN FINISH ─────────────────────────

        if (mirium >= backToSafe)
        {
            FinishRun();
        }

        UpdateUI(mirium);

        UpdateMiriumProgress(mirium);
    }

    // ───────────────────────── RESET RUN MIRIUM ─────────────────────────

    void ResetRunMirium()
    {
        if (playerCollector != null)
        {
            GameManager.Instance.playerRunMirium +=
     playerCollector.collectedMirium;

            playerCollector.collectedMirium = 0;
        }
    }

    // ───────────────────────── FAZE ACTIVATION ─────────────────────────

    void ActivateFaze2()
    {
        currentFaze = 2;

        Debug.Log("FAZE 2 ACTIVATED");

        if (faze2Decorations != null)
        {
            faze2Decorations.SetActive(true);
        }

        OnFazeChanged?.Invoke(currentFaze);

        ResetRunMirium();

        TeleportToRandomLocation();
    }

    void ActivateFaze3()
    {
        currentFaze = 3;

        Debug.Log("FAZE 3 ACTIVATED");

        if (faze3Decorations != null)
        {
            faze3Decorations.SetActive(true);
        }

        OnFazeChanged?.Invoke(currentFaze);

        ResetRunMirium();

        TeleportToRandomLocation();
    }

    // ───────────────────────── RUN FINISH ─────────────────────────

    void FinishRun()
    {
        runFinished = true;

        int[] percentages = { 25, 50, 75 };

        int reward = 0;

        int faze2Percent =
            percentages[
                UnityEngine.Random.Range(
                    0,
                    percentages.Length
                )
            ];

        reward += Mathf.RoundToInt(
            faze2Border * (faze2Percent / 100f)
        );

        int faze3Percent =
            percentages[
                UnityEngine.Random.Range(
                    0,
                    percentages.Length
                )
            ];

        reward += Mathf.RoundToInt(
            faze3Border * (faze3Percent / 100f)
        );

        int safePercent =
            percentages[
                UnityEngine.Random.Range(
                    0,
                    percentages.Length
                )
            ];

        reward += Mathf.RoundToInt(
            backToSafe * (safePercent / 100f)
        );

        GameManager.Instance.mirium += reward;

        RunStatistics.miriumCollected =
            GameManager.Instance.playerRunMirium;

        RunStatistics.phasesReached =
            currentFaze;

        RunStatistics.playerDied = false;

        RunStatistics.rewardEarned = reward;

        Time.timeScale = 0f;

        if (afterBattleUI != null)
        {
            afterBattleUI.Open();
        }

        Debug.Log("RUN FINISHED");
    }

    // ───────────────────────── TELEPORT ─────────────────────────

    void TeleportToRandomLocation()
    {
        if (player == null ||
            randomDestinations == null ||
            randomDestinations.Length == 0)
            return;

        List<Transform> validDestinations =
            new List<Transform>();

        foreach (Transform destination in randomDestinations)
        {
            if (destination != null)
            {
                validDestinations.Add(destination);
            }
        }

        if (validDestinations.Count == 0)
        {
            Debug.LogWarning(
                "No valid random destinations assigned."
            );

            return;
        }

        int randomIndex =
            UnityEngine.Random.Range(
                0,
                validDestinations.Count
            );

        Transform target =
            validDestinations[randomIndex];

        TeleportPlayer(target);
    }

    void TeleportPlayer(Transform target)
    {
        CharacterController cc =
            player.GetComponent<CharacterController>();

        Rigidbody rb =
            player.GetComponent<Rigidbody>();

        if (cc != null)
        {
            cc.enabled = false;
        }

        player.position = target.position;

        player.rotation = target.rotation;

#if UNITY_2017_2_OR_NEWER
        Physics.SyncTransforms();
#endif

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (cc != null)
        {
            cc.enabled = true;

            cc.Move(Vector3.zero);
        }
    }

    // ───────────────────────── UI ─────────────────────────

    void UpdateUI(int mirium)
    {
        if (fazeText != null)
        {
            fazeText.text = $"{currentFaze}";
        }

        if (playerMiriumText != null)
        {
            playerMiriumText.text = $"{mirium}";
        }

        if (mirium_collector_player_text != null)
        {
            mirium_collector_player_text.text = $"{mirium}";
        }

        if (nextFazeBorderText != null)
        {
            if (currentFaze == 1)
            {
                nextFazeBorderText.text =
                    $"{faze2Border}";
            }
            else if (currentFaze == 2)
            {
                nextFazeBorderText.text =
                    $"{faze3Border}";
            }
            else
            {
                nextFazeBorderText.text = "SAFE";
            }
        }
    }

    // ───────────────────────── PROGRESS BAR ─────────────────────────

    void UpdateMiriumProgress(int mirium)
    {
        if (miriumFillImage == null)
            return;

        float progress = 0f;

        if (currentFaze == 1)
        {
            progress =
                (float)mirium / faze2Border;
        }
        else if (currentFaze == 2)
        {
            progress =
                (float)(mirium - faze2Border) /
                (faze3Border - faze2Border);
        }
        else
        {
            progress = 1f;
        }

        progress = Mathf.Clamp01(progress);

        miriumFillImage.fillAmount =
            progress;
    }
}