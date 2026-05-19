using UnityEngine;

public class miriumPlayerCollector : MonoBehaviour
{
    public int collectedMirium = 0;

    public void AddMirium(int amount)
    {
        collectedMirium += amount;
        Debug.Log($"Mirium gained: {amount} | Total: {collectedMirium}");
    }
}