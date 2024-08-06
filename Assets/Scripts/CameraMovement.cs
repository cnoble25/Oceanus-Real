using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform Player;

    private void FixedUpdate()
    {
        transform.position = new Vector3(Player.position.x, 0, transform.position.z);
    }
}
