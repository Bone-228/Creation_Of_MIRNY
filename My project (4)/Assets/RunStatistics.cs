using UnityEngine;

public static class RunStatistics
{
    public static int miriumCollected;

    public static int phasesReached;

    public static bool playerDied;

    public static int rewardEarned;

    public static void ResetStats()
    {
        miriumCollected = 0;

        phasesReached = 0;

        playerDied = false;

        rewardEarned = 0;
    }
}