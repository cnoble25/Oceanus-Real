using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayHomeScript : MonoBehaviour
{

    public PlayerInformation PlayerInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackHome()
    {
        SceneManager.LoadScene("Menu");
    }

    public void BackToGame()
    {
        PlayerInfo.score = 0;
        PlayerInfo.Bombs = 3;
        PlayerInfo.lives = 3;
        SceneManager.LoadScene(1);
    }
}
