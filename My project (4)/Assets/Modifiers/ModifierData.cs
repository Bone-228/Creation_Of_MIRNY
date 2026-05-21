using UnityEngine;

[System.Serializable]
public abstract class ModifierData: ScriptableObject
{
    public string modifierName;

    [TextArea]
    public string description;

    public Sprite icon;

    public int miriumRequired;

}