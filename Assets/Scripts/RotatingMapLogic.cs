using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingMapLogic : MonoBehaviour
{
    [SerializeField] LayerMask rotatableLayer;
    void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.gameObject.layer == 6 && !other.CompareTag("Player"))
        {
            if(other.gameObject.transform.position.x > transform.position.x)
            {
                other.gameObject.transform.position = new Vector3(transform.position.x - transform.lossyScale.x/2, other.gameObject.transform.position.y, other.gameObject.transform.position.z);
            }else if(other.gameObject.transform.position.x < transform.position.x)
            {
                other.gameObject.transform.position = new Vector3(transform.position.x + transform.lossyScale.x/2, other.gameObject.transform.position.y, other.gameObject.transform.position.z);
            }
            else
            {

            }
        }
    }
}
