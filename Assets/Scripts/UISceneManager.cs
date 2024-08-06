using System.Collections;
using System.Collections.Generic;
//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class UISceneManager : MonoBehaviour
{
    public static UISceneManager instance;

    [SerializeField] GameObject Panel;

    [SerializeField] GameObject Player;

    UnityEngine.UI.Image WinImg;

    void Awake(){
        if(instance == null)
        { 
            instance = this;
        }else
        {
            Destroy(this);
        }
        WinImg= Panel.GetComponent<UnityEngine.UI.Image>();
        WinImg.enabled = false;
        Panel.SetActive(false);
    }

    void Update()
    {
        print(JellyFishManager.instance.JellyFish.Count);
         if(JellyFishManager.instance.JellyFish.Count == 0)
         {
            Player.SetActive(false);
         }
    }

}
