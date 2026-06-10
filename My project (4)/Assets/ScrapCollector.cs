using UnityEngine;

public class ScrapCollector : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) 
        {
            playerScrapsCollected += 10000;
        }

        GameManager.Instance.playerScraps = playerScrapsCollected;
    }
    public int playerScrapsCollected = 0;
    public void AddScraps(int scrapAmount) 
    { 
        playerScrapsCollected += scrapAmount;
    }
}
