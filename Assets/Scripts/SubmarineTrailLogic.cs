using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineTrailLogic : MonoBehaviour
{
    [SerializeField] GameObject Player;

    Animator trail;

    void Awake()
    {
        trail = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector2(1.65f * (Player.GetComponent<PlayerLogic>().direction ? -1 : 1), -0.035f);
        GetComponentInParent<SpriteRenderer>().flipX = Player.GetComponent<PlayerLogic>().direction;


        if(trail.GetInteger("Speed") == 0){
            GetComponent<SpriteRenderer>().enabled = false;
        }else
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        trail.SetInteger("Speed", 0);
        if(Mathf.Abs(Player.GetComponent<Rigidbody2D>().velocity.x) > 1)
        {   
            trail.SetInteger("Speed", 1);
            if(Mathf.Abs(Player.GetComponent<Rigidbody2D>().velocity.x) > 3 )
            {
                trail.SetInteger("Speed", 2);
                if(Mathf.Abs(Player.GetComponent<Rigidbody2D>().velocity.x) > 4.2)
                {
                    trail.SetInteger("Speed", 3);
                }
            }
        }
        
    }
}
