
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Permanent Player Currency")]
    public int mirium = 0;


    //STORE MODIFIERS
    [Header("Unlocked Modifiers")]
    public List<ModifierData> unlockedModifiers =
        new List<ModifierData>();

    [Header("Equipped Modifiers")]
    public List<ModifierData> equippedModifiers =
        new List<ModifierData>();
    //STORE GUNS

    [Header("Permanent Scraps Currency")]
    public int playerScraps = 0;

    [Header("Owned Guns")]
    public List<GunData> ownedGuns =
    new List<GunData>();

    [Header("Equipped Guns")]
    public GunData primaryGun;

    public GunData secondaryGun;


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

