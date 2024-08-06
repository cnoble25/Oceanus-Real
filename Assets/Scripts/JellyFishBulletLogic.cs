using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyFishBulletLogic : MonoBehaviour
{
    public Vector3 Direction;

    public Rigidbody2D rb;

    public float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = (Direction.normalized)*speed;
    }
}
