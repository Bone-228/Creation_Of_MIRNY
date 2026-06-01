using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Currencies")]
    public int playerScraps = 0;
    public int mirium = 0;

    [Header("Current Run")]
    public int playerRunMirium = 0;

    // ───────────────────── GUNS ─────────────────────

    [Header("Owned Guns")]
    public List<GunData> ownedGuns = new List<GunData>();

    [Header("Equipped Guns")]
    public GunData primaryGun;
    public GunData secondaryGun;

    // ───────────────────── MODIFIERS ─────────────────────

    [Header("Unlocked Modifiers")]
    public List<ModifierData> unlockedModifiers = new List<ModifierData>();

    [Header("Equipped Modifiers")]
    public List<ModifierData> equippedModifiers = new List<ModifierData>();

    // ───────────────────── EQUIP SLOT SYSTEM ─────────────────────

    public enum EquipSlot
    {
        Primary,
        Secondary
    }

    public EquipSlot selectedEquipSlot;

    // ───────────────────── SINGLETON ─────────────────────

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

    public bool OwnsGun(GunData gun)
    {
        return ownedGuns.Contains(gun);
    }
}