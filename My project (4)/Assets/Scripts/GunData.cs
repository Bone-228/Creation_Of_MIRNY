using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Weapons/Gun Data")]
public class GunData : ScriptableObject
{
    public string gunName;

    public int gunPrice;

    public Sprite gunIcon;
}