using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    private Player player;

    public Slider reputation;
    public Text repText;
    public Slider gathering;
    public Text gatText;
    public Slider combat;
    public Text cbtText;
    public Slider communication;
    public Text comText;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume ()
    {
        reputation.value = (player.repExpToLVL - player.repExp) / player.repExpToLVL;
        repText.text = "REPUTATION: Lvl " + player.reputation.ToString();
        gathering.value = (player.gatExpToLVL - player.gatExp) / player.gatExpToLVL;
        gatText.text = "GATHERING: Lvl " + player.gathering.ToString();
        combat.value = (player.cbtExpToLVL - player.cbtExp) / player.cbtExpToLVL;
        cbtText.text = "COMBAT: Lvl " + player.combat.ToString();
        communication.value = (player.comExpToLVL - player.comExp) / player.comExpToLVL;
        comText.text = "COMMUNICATION: Lvl " + player.communication.ToString();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Quit()
    {
        Application.Quit(0);
    }
}
