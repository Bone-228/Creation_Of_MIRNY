using Assets.scripts_camera;
using TMPro;
using UnityEngine;

public class AfterBattleUI : MonoBehaviour
{
    public GameObject panel;

    public GameObject cursorUI;

    public GameObject lorePanel;

    public TextMeshProUGUI resultText;

    public TextMeshProUGUI miriumText;

    public TextMeshProUGUI phasesText;

    public TextMeshProUGUI rewardText;

    public GameObject battleui;

    public BattleAudioManager battleAudioManager;

    public playerHealthManager playerHealthManager;
    [Header("PLAYER")]
    public PlayerMovement playerMovement;
    public ThirdPersonCam playerCamera;


    private void Start()
    {
        panel.SetActive(true);
        cursorUI.SetActive(true);
        battleui.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        Open();
    }

    public void Open()
    {
        if(RunStatistics.playerDied)
        {
            battleAudioManager.musicSource.PlayOneShot(battleAudioManager.player_dead_sound);
            battleAudioManager.PlayDeathMusic();
        }
        else
        {
            battleAudioManager.PlaySurviveMusic();
        }

        Debug.Log("Opening After Battle UI");

        panel.SetActive(true);
        cursorUI.SetActive(true);
        battleui.SetActive(false);
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


        playerMovement.enabled = false;
        playerCamera.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ContinueToLore()
    {
        panel.SetActive(false);
        cursorUI.SetActive(true);
        battleui.SetActive(false);
        if (lorePanel != null)
        {
            lorePanel.SetActive(true);
        }

        playerMovement.enabled = false; 
        playerCamera.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }
}