using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy", menuName = "ScriptableObjects/Enemies", order = 1)]
public class EnemyScriptableObject : ScriptableObject
{
    public float speed;

    public Sprite sprite;

    public float sinkingSpeed;

    public float ProjectileSpeed;

    public enum EnemyType
    {
        JellyFish,
        RainbowJellyFish
    };
}
