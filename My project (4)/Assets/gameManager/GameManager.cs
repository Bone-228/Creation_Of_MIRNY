using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Currency")]
    public int mirium = 1000;

    [Header("Unlocked Modifiers")]
    public List<ModifierData> unlockedModifiers =
        new List<ModifierData>();

    [Header("Equipped Modifiers")]
    public List<ModifierData> equippedModifiers =
        new List<ModifierData>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}