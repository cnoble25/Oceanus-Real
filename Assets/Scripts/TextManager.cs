using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    //public TextMeshProUGUI AquaDefender;
    public TextMeshProUGUI SpaceToContinue;
    public bool gameBegin;

    public int version;
    
    public void Start()
    {
        SpaceToContinue = GetComponent<TextMeshProUGUI>();
        if(version == 0 || version == 1 || version ==3)
            StartCoroutine(Blinking());
        gameBegin = false;
    }

    // Ensure there is only one Update method
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameBegin = true;
            SceneManager.LoadScene("UIScene");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }

    public IEnumerator Blinking()
    {
        SpaceToContinue = GetComponent<TextMeshProUGUI>();
        while (!gameBegin)
        {
            if (version == 0)
                SpaceToContinue.text = "CLICK TO START";
            else if (version == 1)
                SpaceToContinue.text = "CONTROLS";
            else if (version == 3)
                SpaceToContinue.text = "BACK";
            yield return new WaitForSeconds(0.5f);
            SpaceToContinue.text = "";
            yield return new WaitForSeconds(0.5f);
            
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene("UIScene");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Escape()
    {
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();

    }
}