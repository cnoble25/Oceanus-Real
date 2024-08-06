using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static JellyFishManager;
using UnityEngine.SceneManagement;

public class SharkLogic : MonoBehaviour
{
    [SerializeField] EnemyScriptableObject myEnemy;

    [SerializeField] RuntimeAnimatorController Shark;

    [SerializeField] PlayerInformation PlayerInfo;

    public GameObject Player;
    
    bool isInBombZone;

    [SerializeField] Sprite shark;

    [SerializeField] GameObject JellyFishBullet;

    [SerializeField] GameObject DeathParticles;

    [SerializeField] FMODUnity.EventReference DeathEvent;

    Animator anim;

    Rigidbody2D rb;

    IEnumerator coroutine;

    bool isMoving = false;

    bool dead = false;

    private void Awake()
    {
        //JellyFishManager.instance.JellyFish.Add(gameObject);

        rb = GetComponent<Rigidbody2D>();

        GetComponent<SpriteRenderer>().sprite = myEnemy.sprite;

        anim = GetComponent<Animator>();


    }

    private void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -8, 5));
        if(isMoving)
        {
            MovingLogic();
        }
        if (coroutine == null && !dead && isMoving)
        {
            coroutine = FireDelay();
            StartCoroutine(FireDelay());
        }
    }

     void Update()
    {
        if((JellyFishManager.instance.people.Count == 1 || JellyFishManager.instance.people.Count == 0) &&  SceneManager.GetActiveScene().name == "UIScene")
        {
            StopAllCoroutines();
        }

        print(dead);
        if(Input.GetKeyDown(KeyCode.O) && !dead && isInBombZone && !Player.GetComponent<PlayerLogic>().bombing)
        {
            if(PlayerInfo.Bombs > 0)
            {
            dead = true;
            JellyFishManager.instance.JellyFish.Remove(gameObject);
            anim.SetBool("Died", true);
            StopAllCoroutines();
            StartCoroutine(DeathAnimation());
            }
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ActiveZone") && !dead)
        {
            isMoving = true;
        }
         if(collision.CompareTag("Offscreen"))
        {
            isInBombZone = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Bullet"))
        {
            JellyFishManager.instance.JellyFish.Remove(gameObject);
            Destroy(other.gameObject);
            //Destroy(gameObject);
            anim.SetBool("Died", true);
            StopAllCoroutines();
            StartCoroutine(DeathAnimation());
            dead = true;
        }
        if (other.transform.CompareTag("ActiveZone"))
        {
            isMoving = true;
        }
         if(other.CompareTag("Offscreen"))
        {
            isInBombZone = true;
        }
    }

    IEnumerator FireDelay()
    {

        yield return new WaitForSeconds(2f);
        Vector2 direction = (new Vector2(Player.transform.position.x - transform.position.x, Player.transform.position.y - transform.position.y)).normalized;
        GameObject newBullet = Instantiate(JellyFishBullet, transform.position, Quaternion.identity);
        newBullet.GetComponent<JellyFishBulletLogic>().Direction = direction;
        newBullet.GetComponent<JellyFishBulletLogic>().speed = myEnemy.ProjectileSpeed;


        coroutine = null;

    }

    void MovingLogic()
    {
        if(!dead){
        Vector3 direction = Vector3.MoveTowards(transform.position, new Vector3(Player.transform.position.x + 5 * (transform.position.x > Player.transform.position.x ? 1 : -RotateShark()), Player.transform.position.y + 1, 0), myEnemy.speed / 50);
        transform.position = direction;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("ActiveZone"))
        {
            isMoving = false;
        }
        if(collision.CompareTag("Offscreen"))
        {
            isInBombZone = false;
        }
    }

    int RotateShark()
    {
        GetComponent<SpriteRenderer>().flipX = true;

        return 1;
    }
     IEnumerator DeathAnimation()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().enabled = false;
        DeathParticles.GetComponent<ParticleSystem>().Play();
        FMODUnity.RuntimeManager.PlayOneShot(DeathEvent, transform.position);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
