using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFishManager : MonoBehaviour
{
    public List<GameObject> people;
    public List<GameObject> JellyFish;

    public enum EnemyState
    {
        Idle,
        Moving,
        Sinking,
        Dead
    }

    public static JellyFishManager instance;

    private void Awake()
    {
        people = new List<GameObject>();
         if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
