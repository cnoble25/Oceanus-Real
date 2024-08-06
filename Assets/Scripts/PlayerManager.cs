using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   public enum PlayerState
    {
        Alive,
        Dead
    }

    public PlayerState state;

    public static PlayerManager instance;

    private void Awake()
    {
        if(instance == null) { instance = this; } else { Destroy(this); }
    }


    public void UpdatePlayerState(PlayerState newState)
    {
        state = newState;

        switch(state)
        {
            case PlayerState.Alive:
                AliveState();
                break;

            case PlayerState.Dead:
                DeadState();
                break;
        }
    }

    void AliveState()
    {

    }

    void DeadState()
    {

    }
}
