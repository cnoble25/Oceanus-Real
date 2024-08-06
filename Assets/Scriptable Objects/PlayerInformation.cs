using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Info", menuName = "ScriptableObjects/PlayerInfo", order = 1)]
public class PlayerInformation : ScriptableObject
{
    public int lives;

    public int score;

    public int Bombs;

    public bool noLegacyControls;
}
