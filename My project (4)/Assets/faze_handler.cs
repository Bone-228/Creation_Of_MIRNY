
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

    [Header("References")]
    public miriumPlayerCollector playerCollector;
    public Transform player;

    [Header("Teleport Destinations")]
    public Transform[] randomDestinations;

    [Header("UI")]
    public TextMeshProUGUI fazeText;
    public TextMeshProUGUI playerMiriumText;
    public TextMeshProUGUI nextFazeBorderText;

    [Tooltip("Additional mirium amount text.")]
    public TextMeshProUGUI mirium_collector_player_text;

    [Header("Mirium Progress Bar")]
    public Image miriumFillImage;

    [Header("FAZE 2 DECORATIONS")]
    public GameObject faze2Decorations;

    [Header("FAZE 3 DECORATIONS")]
    public GameObject faze3Decorations;

    private int currentFaze = 1;

    void Update()
    {
        if (playerCollector == null)
            return;

        int mirium = playerCollector.collectedMirium;

        if (currentFaze == 1 && mirium >= faze2Border)
        {
            ActivateFaze2();
        }
        else if (currentFaze == 2 && mirium >= faze3Border)
        {
            ActivateFaze3();
        }

        UpdateUI(mirium);
        UpdateMiriumProgress(mirium);
    }

    void ActivateFaze2()
    {
        currentFaze = 2;

        Debug.Log("FAZE 2 ACTIVATED");

        if (faze2Decorations != null)
            faze2Decorations.SetActive(true);

        OnFazeChanged?.Invoke(currentFaze);

        TeleportToRandomLocation();
    }

    void ActivateFaze3()
    {
        currentFaze = 3;

        Debug.Log("FAZE 3 ACTIVATED");

        if (faze3Decorations != null)
            faze3Decorations.SetActive(true);

        OnFazeChanged?.Invoke(currentFaze);

        TeleportToRandomLocation();
    }

    void TeleportToRandomLocation()
    {
        if (player == null || randomDestinations == null || randomDestinations.Length == 0)
            return;

        List<Transform> validDestinations = new List<Transform>();

        foreach (Transform destination in randomDestinations)
        {
            if (destination != null)
            {
                validDestinations.Add(destination);
            }
        }

        if (validDestinations.Count == 0)
        {
            Debug.LogWarning("No valid random destinations assigned.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, validDestinations.Count);

        Transform target = validDestinations[randomIndex];

        TeleportPlayer(target);
    }

    void TeleportPlayer(Transform target)
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (cc != null)
            cc.enabled = false;

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

    void UpdateUI(int mirium)
    {
        if (fazeText != null)
            fazeText.text = $"{currentFaze}";

        if (playerMiriumText != null)
            playerMiriumText.text = $"{mirium}";

        if (mirium_collector_player_text != null)
            mirium_collector_player_text.text = $"{mirium}";

        if (nextFazeBorderText != null)
        {
            if (currentFaze == 1)
                nextFazeBorderText.text = $"{faze2Border}";
            else if (currentFaze == 2)
                nextFazeBorderText.text = $"{faze3Border}";
            else
                nextFazeBorderText.text = "MAX";
        }
    }

    void UpdateMiriumProgress(int mirium)
    {
        if (miriumFillImage == null)
            return;

        float progress = 0f;

        if (currentFaze == 1)
        {
            progress = (float)mirium / faze2Border;
        }
        else if (currentFaze == 2)
        {
            progress = (float)(mirium - faze2Border) / (faze3Border - faze2Border);
        }
        else
        {
            progress = 1f;
        }

        progress = Mathf.Clamp01(progress);

        miriumFillImage.fillAmount = progress;
    }
}
