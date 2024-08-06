using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public PlayerInformation PlayerInfo;

    public static MainMenu mm;

    [SerializeField] GameObject ToggleButton;

    [SerializeField] GameObject instructions;

    public void Awake()
    {
        if(mm)
        {
            Destroy(this.gameObject);
        }
        else
        {
            mm = this;
        }
    }
    public void StartGame()
    {
        PlayerInfo.score = 0;
        PlayerInfo.Bombs = 3;
        PlayerInfo.lives = 3;
        SceneManager.LoadScene(1);
      
    }

    public void InstructionPage()
    {
        SceneManager.LoadScene("Instructions");

    }

    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void WinScene()
    {
        Debug.Log("HEY IN MAIN MENU");
    }

    public void BackHome()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoDeeper()
    {
        SceneManager.LoadScene("DeeperWater");
    }

    public void ToggleLegacy()
    {
        PlayerInfo.noLegacyControls = !PlayerInfo.noLegacyControls;
        if(PlayerInfo.noLegacyControls){
            ToggleButton.GetComponent<TextMeshProUGUI>().text = "Toggle Legacy Controls: New";
            instructions.GetComponent<TextMeshProUGUI>().text = "W - MOVE UP \n\n A - MOVE LEFT \n\n S - MOVE DOWN \n\n D - MOVE RIGHT \n\n I - SHOOT \n\n O - TELEPORT \n\n P - BOMB";
        }
        if(!PlayerInfo.noLegacyControls){
            ToggleButton.GetComponent<TextMeshProUGUI>().text = "Toggle Legacy Controls: Legacy";
            instructions.GetComponent<TextMeshProUGUI>().text = "W - MOVE UP \n\n A - SWITCH HORIZONTAL DIRECTION \n\n S - MOVE DOWN \n\n D - MOVE HORIZONTALLY \n\n I - SHOOT \n\n O - TELEPORT \n\n P - BOMB";
        }
    }
}
