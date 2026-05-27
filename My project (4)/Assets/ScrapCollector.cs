using UnityEngine;

public class ScrapCollector : MonoBehaviour
{

    public int playerScrapsCollected = 0;
    public void AddScraps(int scrapAmount) 
    { 
        playerScrapsCollected += scrapAmount;
    }
}
