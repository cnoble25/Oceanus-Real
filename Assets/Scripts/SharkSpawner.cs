using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SharkSpawner : MonoBehaviour
{

    [SerializeField] GameObject Shark;

    [SerializeField] float Range;

    [SerializeField] float timer;
   void Awake()
   {
        StartCoroutine(SharkTimer(Range));
   }

   IEnumerator SharkTimer(float Magnitude)
   {
        yield return new WaitForSeconds(timer);
        float angle = UnityEngine.Random.Range(0,360)*Mathf.PI/180;
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);
        
        GameObject Temp = Instantiate(Shark, (new Vector2(transform.position.x + x, transform.position.y + y))*Magnitude, Quaternion.identity);
        Temp.GetComponent<SharkLogic>().Player = transform.parent.gameObject;
   }
}
