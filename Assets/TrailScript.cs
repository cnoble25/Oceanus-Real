using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    public PlayerLogic player;

    [SerializeField] ParticleSystem movementParticle;

    

    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (player.GetComponent<Rigidbody2D>().velocity != Vector2.zero)
        {
            movementParticle.Play();
            
        }
        else
        {
            movementParticle.Stop();
        }
       
    }
}
