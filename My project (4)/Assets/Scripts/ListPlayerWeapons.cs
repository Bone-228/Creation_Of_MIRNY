using System;
using System.Reflection;
using UnityEngine;

public class ListPlayerWeapons : MonoBehaviour
{
    [Tooltip("Optional: assign your Player GameObject here. If left empty the script will try to find a GameObject named 'PlayerObject' or tagged 'Player'.")]
    public GameObject playerObject;

    void Start()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.Find("PlayerObject");
        }

        if (playerObject == null)
        {
            playerObject = GameObject.FindWithTag("Player");
        }

        if (playerObject == null)
        {
            Debug.LogWarning("ListPlayerWeapons: No player object found (looked for 'PlayerObject' and tag 'Player').");
            return;
        }

        // This requires a `Weapon` base class to exist in the project.
        var weapons = playerObject.GetComponentsInChildren<Weapon>(true);
        if (weapons == null || weapons.Length == 0)
        {
            Debug.Log("ListPlayerWeapons: No Weapon-derived components found under the player.");
            return;
        }

        Debug.Log($"ListPlayerWeapons: Found {weapons.Length} Weapon-derived component(s):");
        for (int i = 0; i < weapons.Length; i++)
        {
            var w = weapons[i];
            if (w == null) continue;

            var t = w.GetType();
            int assignedId = -1;

            try
            {
                // Try common property names first (prefer property)
                var prop = t.GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                           ?? t.GetProperty("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                           ?? t.GetProperty("WeaponId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                           ?? t.GetProperty("weaponId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (prop != null && prop.PropertyType == typeof(int))
                {
                    var setMethod = prop.GetSetMethod(true);
                    if (setMethod != null)
                    {
                        // Use the setter even if non-public
                        setMethod.Invoke(w, new object[] { i });
                        var val = prop.GetValue(w);
                        if (val is int iv) assignedId = iv;
                    }
                }
                else
                {
                    // Try fields
                    var field = t.GetField("id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                ?? t.GetField("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                ?? t.GetField("weaponId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                ?? t.GetField("WeaponId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    if (field != null && field.FieldType == typeof(int))
                    {
                        field.SetValue(w, i);
                        var val = field.GetValue(w);
                        if (val is int iv) assignedId = iv;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"ListPlayerWeapons: Failed to assign id to component '{w.name}' of type '{t.FullName}': {ex.Message}");
            }

            Debug.Log($"- Index: {i}, AssignedId: {assignedId}, Type: {t.FullName}, Component name: {w.name}, Path: {GetTransformPath(w.transform)}");
        }
    }

    private string GetTransformPath(Transform t)
    {
        if (t == null) return string.Empty;
        string path = t.name;
        var parent = t.parent;
        while (parent != null)
        {
            path = parent.name + "/" + path;
            parent = parent.parent;
        }
        return path;
    }
}