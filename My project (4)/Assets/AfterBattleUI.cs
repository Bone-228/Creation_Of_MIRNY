using TMPro;
using UnityEngine;

public class AfterBattleUI : MonoBehaviour
{
    public GameObject panel;

    public GameObject lorePanel;

    public TextMeshProUGUI resultText;

    public TextMeshProUGUI miriumText;

    public TextMeshProUGUI phasesText;

    public TextMeshProUGUI rewardText;

    private void Start()
    {
        panel.SetActive(false);

        if (lorePanel != null)
        {
            lorePanel.SetActive(false);
        }
    }

    public void Open()
    {
        panel.SetActive(true);

        resultText.text =
            RunStatistics.playerDied
            ? "DIED"
            : "ESCAPED";

        miriumText.text =
            RunStatistics.miriumCollected.ToString();

        phasesText.text =
            RunStatistics.phasesReached.ToString();

        rewardText.text =
            RunStatistics.rewardEarned.ToString();
    }

    public void ContinueToLore()
    {
        panel.SetActive(false);

        if (lorePanel != null)
        {
            lorePanel.SetActive(true);
        }
    }
}