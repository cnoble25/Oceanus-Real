using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffScreenLogic : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") || other.CompareTag("enemyBullet"))
        {
            Destroy(other.gameObject);
        }
    }
}
