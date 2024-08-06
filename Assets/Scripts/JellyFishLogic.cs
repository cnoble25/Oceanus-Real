using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JellyFishLogic : MonoBehaviour
{

    public JellyFishManager.EnemyState enemyState;

    [SerializeField] EnemyScriptableObject myEnemy;

    Rigidbody2D rb;

    [SerializeField] int ptWorth;

    [SerializeField] RuntimeAnimatorController JellyFish;

    [SerializeField] PlayerInformation PlayerInfo;

    Animator anim;

    [SerializeField] GameObject Player;

    [SerializeField] GameObject DeathParticles;

    [SerializeField] GameObject JellyFishBullet;

    GameObject HeldCivilian;

    [SerializeField] FMODUnity.EventReference DeathEvent;

    [SerializeField] FMODUnity.EventReference fireEvent;

    [SerializeField] GameObject WhirlPool;

    [SerializeField] GameObject DeepZoneText;

    IEnumerator coroutine;

    public bool isInBombZone;
    
    int canGo = 0;
    
    int possibleCivilian = 0;

    public List <GameObject> CivilianAr;

    bool isOnFloor = false;

    public bool dyingProcess = false;

    public GameObject Background1;
    public GameObject Background2;
    public GameObject Background3;
    public GameObject Background4;
    public GameObject winningBG;

    public TextMeshProUGUI winText;

    public GameObject theMiniMap;

    public GameObject replayBox;
    public GameObject homeBox;
    public GameObject replayButton;
    public GameObject homeButton;

    //public Animation sub;

    // Start is called before the first frame update
    void Start()
    {
        winText.enabled = false;
        winningBG.SetActive(false);

        replayBox.SetActive(false);
        homeBox.SetActive(false);
        replayButton.SetActive(false);
        homeButton.SetActive(false);

        JellyFishManager.instance.JellyFish.Add(gameObject);

        enemyState = JellyFishManager.EnemyState.Idle;

        rb = GetComponent<Rigidbody2D>();

        GetComponent<SpriteRenderer>().sprite = myEnemy.sprite;

        GetComponent<Animator>().runtimeAnimatorController = JellyFish;

        anim = GetComponent<Animator>();

        resetCivAr();

        //sub.gameObject.GetComponent<Animation>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O) && !dyingProcess && isInBombZone)
        {
            print(isInBombZone + ", " + gameObject.name);
            if(PlayerInfo.Bombs > 0)
            {
            dyingProcess = true;
            JellyFishManager.instance.JellyFish.Remove(gameObject);
            if (HeldCivilian != null) 
            { 
                JellyFishManager.instance.people.Add(HeldCivilian);
                HeldCivilian.GetComponent<CivilianLogic>().isOnCeiling = false;
                HeldCivilian.GetComponent<CivilianLogic>().Holder = null;
                HeldCivilian.transform.parent = null; 
            }
            anim.SetBool("Died", true);
            StopAllCoroutines();
            StartCoroutine(DeathAnimation());
            enemyState = JellyFishManager.EnemyState.Dead;
            }
        }
        if((JellyFishManager.instance.people.Count == 1 || JellyFishManager.instance.people.Count == 0) &&  SceneManager.GetActiveScene().name == "UIScene")
        {
            StopCoroutine(coroutine);
            StopCoroutine(FireDelay());
        }

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(CivilianAr.Count != JellyFishManager.instance.people.Count || Vector2.Distance(transform.position, CivilianAr[possibleCivilian].transform.position) > 20)
        {
            resetCivAr();
        }
        checkState();
        if (coroutine == null && enemyState != JellyFishManager.EnemyState.Idle && enemyState != JellyFishManager.EnemyState.Dead)
        {
            coroutine = FireDelay();
            StartCoroutine(FireDelay());
        }
        if(isOnFloor && rb.velocity.y < 0){
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        
    }

    void checkState()
    {
        switch(enemyState)
        {
            case JellyFishManager.EnemyState.Dead:
                DeadLogic();
                break;
            case JellyFishManager.EnemyState.Moving:
                MovingLogic();
                break;
            case JellyFishManager.EnemyState.Sinking:
                SinkingLogic();
                break;
            case JellyFishManager.EnemyState.Idle:
                IdleLogic();
                break;
        }
    }

    void IdleLogic()
    {
        rb.velocity = Vector3.zero;
    }

    void MovingLogic()
    {
        GetComponent<SpriteRenderer>().flipY = false;
        GameObject NearestCivilian = null;
        if(canGo == 0) NearestCivilian = CivilianAr[0];
        if(canGo == -1 || canGo == 1) NearestCivilian = CivilianAr[possibleCivilian];
       
        if (NearestCivilian != null)
        {

            const double E = 2.718281828459045235360287471352;

            Vector2 NearestCivilianPositionToGoTo = new Vector2(NearestCivilian.transform.position.x, NearestCivilian.transform.position.y-(float)(E));
            Vector2 direction = Vector2.MoveTowards(transform.position, NearestCivilianPositionToGoTo, myEnemy.speed / 50);
            
            if(Vector2.Distance(direction, NearestCivilianPositionToGoTo) < 0.1)
            {
                float guessNumberJellyFish = (int)((UnityEngine.Random.Range(1, 5))+0.1f);
                if(guessNumberJellyFish == 2 || canGo == 1)
                {
                    canGo = 1;
                }else{
                    canGo = -1;
                    possibleCivilian++;
                }
            }
            if(canGo == 1) direction = Vector2.MoveTowards(transform.position, NearestCivilian.transform.position, myEnemy.speed / 50);
            if(canGo == -1 && Vector2.Distance(direction, NearestCivilianPositionToGoTo) < 0.1) {possibleCivilian++;}
            if(CivilianAr.Count-1 < possibleCivilian) possibleCivilian = 0;

            //print(canGo);
            transform.position = direction;


        }
    }

    void DeadLogic()
    {
        rb.velocity = Vector3.zero;
    }

    void SinkingLogic()
    {
        rb.velocity = new Vector3(0, -myEnemy.sinkingSpeed, 0);
        HeldCivilian.transform.localPosition = new Vector3(0, 0 + 1/2, 0);
        GetComponent<SpriteRenderer>().flipY = true;
    }

    List<GameObject> FindNearestCivilian()
    {

        GameObject[] people = JellyFishManager.instance.people.ToArray();
        List<GameObject> sortedPeople = people.OrderBy(p => Vector2.Distance(p.transform.position, transform.position)).ToList();
        return sortedPeople;

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
        if (other.transform.CompareTag("Civilian") && HeldCivilian == null && other.transform.gameObject.GetComponent<CivilianLogic>().Holder == null)
        {
            HeldCivilian = other.gameObject;
            HeldCivilian.GetComponent<CivilianLogic>().Holder = gameObject;
            HeldCivilian.transform.parent = transform;
            HeldCivilian.transform.localPosition = new Vector3(0, 0 + transform.localScale.y / 2, 0);
            JellyFishManager.instance.people.Remove(HeldCivilian);
            enemyState = JellyFishManager.EnemyState.Sinking;
            checkState();
        }

        if (other.transform.CompareTag("Bullet") && enemyState != JellyFishManager.EnemyState.Dead && !dyingProcess)
        {
            dyingProcess = true;
            JellyFishManager.instance.JellyFish.Remove(gameObject);
            if (HeldCivilian != null) 
            { 
                JellyFishManager.instance.people.Add(HeldCivilian);
                HeldCivilian.GetComponent<CivilianLogic>().isOnCeiling = false;
                HeldCivilian.GetComponent<CivilianLogic>().Holder = null;
                HeldCivilian.transform.parent = null; 
            }
            Destroy(other.gameObject);
            anim.SetBool("Died", true);
            StopAllCoroutines();
            StartCoroutine(DeathAnimation());
            enemyState = JellyFishManager.EnemyState.Dead;
        }
        if (other.transform.CompareTag("ActiveZone"))
        {
            enemyState = JellyFishManager.EnemyState.Moving;
            if(HeldCivilian != null) enemyState = JellyFishManager.EnemyState.Sinking;
        }
        if(other.CompareTag("Offscreen"))
        {
            isInBombZone = true;
        }
         if (other.transform.CompareTag("CivilianDeathZone") && enemyState == JellyFishManager.EnemyState.Sinking)
        {
            JellyFishManager.instance.people.Remove(HeldCivilian);
            Destroy(HeldCivilian.gameObject);
        if((JellyFishManager.instance.people.Count == 1 || JellyFishManager.instance.people.Count == 0) &&  SceneManager.GetActiveScene().name == "UIScene")
        {
            StartCoroutine(DeepZone());
        }
        else
        {
            GetComponent<SpriteRenderer>().flipY = false;
            rb.velocity = Vector3.zero;
            enemyState = JellyFishManager.EnemyState.Moving;
            GetComponent<MutantJellyFishLogic>().enabled = true;
            Destroy(this);
        }
        }
        if (other.transform.CompareTag("CivilianDeathZone")){
            isOnFloor = true;
         }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("ActiveZone"))
        {
            enemyState = JellyFishManager.EnemyState.Idle;
        }
         if (collision.transform.CompareTag("CivilianDeathZone")){
            isOnFloor = false;
         }
         if(collision.CompareTag("Offscreen"))
        {
            isInBombZone = false;
        }
    }

    IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(3.5f);
        Vector2 direction = (new Vector2(Player.transform.position.x - transform.position.x, Player.transform.position.y - transform.position.y)).normalized;
        GameObject newBullet = Instantiate(JellyFishBullet, transform.position, Quaternion.identity);
        newBullet.GetComponent<JellyFishBulletLogic>().Direction = direction;
        newBullet.GetComponent<JellyFishBulletLogic>().speed = myEnemy.ProjectileSpeed;
        FMODUnity.RuntimeManager.PlayOneShot(fireEvent, transform.position);
        coroutine = null;
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
        if (count == 0)
        {
            Debug.Log("ABOUT TO CHANGE BG's");
            Background1.SetActive(false);
            Background2.SetActive(false);
            Background3.SetActive(false);
            Background4.SetActive(false);
            winningBG.SetActive(true);
            winningBG.GetComponent<Animation>().Play("SubmarineAnimation");
            winText.enabled = true;
            winText.text = "ATTACK WAVE 1" +
                "\n" + "COMPLETED";
            theMiniMap.SetActive(false);

            replayBox.SetActive(true);
            homeBox.SetActive(true);
            replayButton.SetActive(true);
            homeButton.SetActive(true);
            //Time.timeScale = 0;

            //sub.Play("SubmarineAnimation");
        }
        Destroy(gameObject);
        yield return new WaitForSeconds(1f);
        
        Debug.Log(count);
    }

    IEnumerator DeepZone()
    {
            DeepZoneText.GetComponent<TextMeshProUGUI>().enabled = true;
            yield return new WaitForSeconds(5f);
            WhirlPool.GetComponent<UnityEngine.UI.Image>().enabled = true;
            WhirlPool.GetComponent<Animator>().SetBool("WhirlPool", true);
            yield return new WaitForSeconds(0.5f);
            JellyFishManager.instance.JellyFish.Clear();
            MainMenu.mm.GoDeeper();
    }

    void resetCivAr()
    {
        CivilianAr = FindNearestCivilian();
        possibleCivilian = 0;
    }
}
