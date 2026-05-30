using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Guns/Gun Data")]
public class GunData : ScriptableObject
{
    public string gunName;
    public Sprite gunIcon;
    public int gunPrice;

    public string weaponID;
}