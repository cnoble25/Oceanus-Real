using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MutantJellyFishLogic : MonoBehaviour
{
    public JellyFishManager.EnemyState enemyState;

    [SerializeField] EnemyScriptableObject myEnemy;

    [SerializeField] RuntimeAnimatorController MutantJellyFish;

    [SerializeField] PlayerInformation PlayerInfo;

    [SerializeField] int ptWorth = 200;

    bool isInBombZone;

    [SerializeField] GameObject Player;

    [SerializeField] GameObject JellyFishBullet;

    [SerializeField] GameObject DeathParticles;

    [SerializeField] FMODUnity.EventReference DeathEvent;

    [SerializeField] FMODUnity.EventReference fireEvent;

    public GameObject Background1;
    public GameObject Background2;
    public GameObject Background3;
    public GameObject Background4;
    public GameObject winningBG;

    public TextMeshProUGUI winText;


    Animator anim;


    Rigidbody2D rb;

    IEnumerator coroutine;

    public bool dyingProcess = false;

    public GameObject theMiniMap;

    public GameObject replayBox;
    public GameObject homeBox;
    public GameObject replayButton;
    public GameObject homeButton;


    private void Awake()
    {
        winText.enabled = false;
        winningBG.SetActive(false);

        replayBox.SetActive(false);
        homeBox.SetActive(false);
        replayButton.SetActive(false);
        homeButton.SetActive(false);

        rb = GetComponent<Rigidbody2D>();

        GetComponent<SpriteRenderer>().sprite = myEnemy.sprite;

        anim = GetComponent<Animator>();


    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "DeeperWater")
            JellyFishManager.instance.JellyFish.Add(gameObject);
    }

    void Update()
    {
        if((JellyFishManager.instance.people.Count == 1 || JellyFishManager.instance.people.Count == 0) &&  SceneManager.GetActiveScene().name == "UIScene")
        {
            StopAllCoroutines();
        }
        if(Input.GetKeyDown(KeyCode.O) && !dyingProcess && isInBombZone)
        {
            if(PlayerInfo.Bombs > 0)
            {
            dyingProcess = true;
            JellyFishManager.instance.JellyFish.Remove(gameObject);
            anim.SetBool("Died", true);
            StopAllCoroutines();
            StartCoroutine(DeathAnimation());
            enemyState = JellyFishManager.EnemyState.Dead;
            }
        }

    }

    void FixedUpdate()
    {
        GetComponent<Animator>().runtimeAnimatorController = MutantJellyFish;
        transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, -7f, 3.5f));
        checkState();
        if (coroutine == null && enemyState != JellyFishManager.EnemyState.Idle && enemyState != JellyFishManager.EnemyState.Dead && isInBombZone)
        {
            coroutine = FireDelay();
            StartCoroutine(FireDelay());
        }
    }

    void checkState()
    {
        switch (enemyState)
        {
            case JellyFishManager.EnemyState.Dead:
                DeadLogic();
                break;
            case JellyFishManager.EnemyState.Moving:
                MovingLogic();
                break;
            case JellyFishManager.EnemyState.Sinking:
                IdleLogic();
                break;
            case JellyFishManager.EnemyState.Idle:
                IdleLogic();
                break;
        }
    }

    void DeadLogic()
    {

    }

    void IdleLogic()
    {
        rb.velocity = Vector3.zero;
    }

    void MovingLogic()
    { 
            Vector3 direction =  Vector3.MoveTowards(transform.position, new Vector3(Player.transform.position.x, Player.transform.position.y + 5*(transform.position.y > Player.transform.position.y ? 1 : -1), 0), myEnemy.speed / 50);
            transform.position = direction;
    }


    IEnumerator FireDelay()
    {

        yield return new WaitForSeconds(2.5f);
        Vector2 direction = (new Vector2(Player.transform.position.x - transform.position.x, Player.transform.position.y - transform.position.y)).normalized;
        GameObject newBullet = Instantiate(JellyFishBullet, transform.position, Quaternion.identity);
        newBullet.GetComponent<JellyFishBulletLogic>().Direction = direction;
        newBullet.GetComponent<JellyFishBulletLogic>().speed = myEnemy.ProjectileSpeed;
        FMODUnity.RuntimeManager.PlayOneShot(fireEvent, transform.position);



        coroutine = null;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("ActiveZone") && enemyState != JellyFishManager.EnemyState.Sinking && enemyState != JellyFishManager.EnemyState.Dead)
        {
            enemyState = JellyFishManager.EnemyState.Moving;
        }
        if(collision.CompareTag("Offscreen"))
        {
            isInBombZone = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Bullet") && enemyState != JellyFishManager.EnemyState.Dead)
        {
            dyingProcess = true;
            JellyFishManager.instance.JellyFish.Remove(gameObject);
            Destroy(other.gameObject);
            GetComponent<BoxCollider2D>().isTrigger = true;
            anim.SetBool("Died", true);
            StopAllCoroutines();
            StartCoroutine(DeathAnimation());
            enemyState = JellyFishManager.EnemyState.Dead;
        }
        if (other.transform.CompareTag("ActiveZone"))
        {
            enemyState = JellyFishManager.EnemyState.Moving;
        }
        if(other.CompareTag("Offscreen"))
        {
            isInBombZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("ActiveZone"))
        {
            enemyState = JellyFishManager.EnemyState.Idle;
        }
        if(collision.CompareTag("Offscreen"))
        {
            isInBombZone = false;
        }
    }

    IEnumerator DeathAnimation()
    {
        PlayerInfo.score += ptWorth;
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().enabled = false;
        DeathParticles.GetComponent<ParticleSystem>().Play();
        FMODUnity.RuntimeManager.PlayOneShot(DeathEvent, transform.position);
        yield return new WaitForSeconds(3f);
        int count = JellyFishManager.instance.JellyFish.Count;
        Debug.Log(count);
        if (count == 0)
        {
            Background1.SetActive(false);
            Background2.SetActive(false);
            Background3.SetActive(false);
            Background4.SetActive(false);
            winningBG.SetActive(true);
            winText.enabled = true;
            theMiniMap.SetActive(false);

            replayBox.SetActive(true);
            homeBox.SetActive(true);
            replayButton.SetActive(true);
            homeButton.SetActive(true);
            //Time.timeScale = 0;
        }
        Destroy(gameObject);
        yield return new WaitForSeconds(1f);
        
    }
}
