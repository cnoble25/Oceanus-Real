using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameramini : MonoBehaviour
{
    [SerializeField] Transform Player;

    private void FixedUpdate()
    {
        transform.position = new Vector3(Player.position.x, 26.4f, transform.position.z);
    }
}
