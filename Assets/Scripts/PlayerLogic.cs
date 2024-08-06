using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class PlayerLogic : MonoBehaviour
{

    [SerializeField] float VerticalSpeed;

    [SerializeField] GameObject Bullet;

    [SerializeField] PlayerInformation PlayerInfo;

    [SerializeField] float HorizontalForceContant;

    [SerializeField] float MaxHorizontalSpeed;

    [SerializeField] GameObject DeathParticles;

    [SerializeField] List<GameObject> Bombs = new List<GameObject>();

    [SerializeField] int BombsCount = 3;
    
    [SerializeField] FMODUnity.EventReference fireEvent;

    [SerializeField] FMODUnity.EventReference DeathEvent;

    [SerializeField] FMODUnity.EventReference DeathEventFirstTime;

    [SerializeField] FMODUnity.StudioEventEmitter MovingSounds;

    [SerializeField] FMODUnity.EventReference Teleport;

    [SerializeField] GameObject BackgroundManager;

    [SerializeField] GameObject DeepZoneText;
    
    [SerializeField] GameObject MapSize;

    [SerializeField] GameObject WhirlPoolAnimation;

    [SerializeField] Scene DeepZone;

    Animator anim;

    Rigidbody2D rb;

    [SerializeField] float ProjectileSpeed;

    public bool direction = true;

    GameObject HeldCivilian;

    private MyInput inputAction;

    public InputAction fire;

    AudioSource audioSource;

    SpriteRenderer spriteRenderer;

    Animator SubmarineMoving;

    bool keypressed;

    [SerializeField] float WhirlPoolspeed;

    [SerializeField] GameObject ScoreCounter;

    [SerializeField] GameObject LiveCounter;

    [SerializeField] GameObject BombsCounter;

    [SerializeField] GameObject BombFlash;

    public bool bombing;

    bool dying = false;

    bool fireCoolDown;

    float whirlPoolLocation = 0;

    float timeScale = 0;
    bool inWhirlPool = false;

    [SerializeField] public GameObject loserBG;

    public TextMeshProUGUI loseText;

    public GameObject theMiniMap;

    public GameObject replayBox;
    public GameObject homeBox;
    public GameObject replayButton;
    public GameObject homeButton;


    void Start()
    {
        loseText.enabled = false;
        loserBG.SetActive(false);

        replayBox.SetActive(false);
        homeBox.SetActive(false);
        replayButton.SetActive(false);
        homeButton.SetActive(false);

        rb = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();

        SubmarineMoving = GetComponent<Animator>();

        anim = GetComponent<Animator>();
    }

    private void Awake()
    {
        if(PlayerInfo.lives == 3) FMODUnity.RuntimeManager.PlayOneShot(DeathEventFirstTime, transform.position);
        if(inputAction == null) inputAction = new MyInput();
        fire = inputAction.Player.Fire;
        spriteRenderer = GetComponent<SpriteRenderer>();
        MovingSounds = GetComponent<FMODUnity.StudioEventEmitter>();
        //WhirlPoolAnimation.GetComponent<Animation>()["WhirlPoolAnimation"].wrapMode = WrapMode.Once;
        StartCoroutine(EcologicalDeadZoneCountDown());
        
    }

    private void Update()
    {
        if(PlayerInfo.lives <= 0)
        {
            GameJover();
        }
        //print(PlayerInfo.Bombs);
        if(Input.GetKeyDown(KeyCode.O) && PlayerInfo.Bombs > 0 && !bombing)
        { 
            PlayerInfo.Bombs--; 
            bombing = true;
            StartCoroutine(BombDelay());
        }
        if(!PlayerInfo.noLegacyControls){
        if (Input.GetKeyDown(KeyCode.A) && !dying)
        {
            direction = !direction;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        }
        if(Input.GetKeyDown(KeyCode.I) && !dying && !fireCoolDown){
            fireCoolDown = true;
            Fire();
            StartCoroutine(Delay());
            //FMOD.Studio.EventInstance FireInstance;
             FMODUnity.RuntimeManager.PlayOneShot(fireEvent, transform.position);
        }
        if((Input.GetKeyDown(KeyCode.P) || inWhirlPool) & !dying)
        {
            inWhirlPool = WhirlPool();
        }

        ScoreCounter.GetComponent<TextMeshProUGUI>().text = PlayerInfo.score.ToString();
        LiveCounter.GetComponent<TextMeshProUGUI>().text = "LIVES: " + PlayerInfo.lives.ToString();
        BombsCounter.GetComponent<TextMeshProUGUI>().text = "BOMBS: " + PlayerInfo.Bombs.ToString();

        switch(PlayerInfo.Bombs)
        {
            case 3:
                Bombs[0].GetComponent<UnityEngine.UI.RawImage>().enabled = true;
                Bombs[1].GetComponent<UnityEngine.UI.RawImage>().enabled = true;
                Bombs[2].GetComponent<UnityEngine.UI.RawImage>().enabled = true;
            break;
            case 2:
                Bombs[0].GetComponent<UnityEngine.UI.RawImage>().enabled = true;
                Bombs[1].GetComponent<UnityEngine.UI.RawImage>().enabled = true;
                Bombs[2].GetComponent<UnityEngine.UI.RawImage>().enabled = false;
            break;
            case 1:
                Bombs[0].GetComponent<UnityEngine.UI.RawImage>().enabled = true;
                Bombs[1].GetComponent<UnityEngine.UI.RawImage>().enabled = false;
                Bombs[2].GetComponent<UnityEngine.UI.RawImage>().enabled = false;
            break;
            case 0:
                Bombs[0].GetComponent<UnityEngine.UI.RawImage>().enabled = false;
                Bombs[1].GetComponent<UnityEngine.UI.RawImage>().enabled = false;
                Bombs[2].GetComponent<UnityEngine.UI.RawImage>().enabled = false;
            break;
            default:
            break;
        }
    }


    void FixedUpdate()
    {
        fire = inputAction.Player.Fire;
        if (PlayerManager.instance.state == PlayerManager.PlayerState.Alive && !dying)
        {
            if(!PlayerInfo.noLegacyControls)
            {
            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(MaxHorizontalSpeed*(direction ? 1 : -1), rb.velocity.y), 3*Time.deltaTime);
            
            }else{
                
            }
            }else{
                if (Input.GetKey(KeyCode.D))
                {
                    rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(MaxHorizontalSpeed*1, rb.velocity.y), 3*Time.deltaTime);
                    direction = true;
                    spriteRenderer.flipX = false;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(MaxHorizontalSpeed*-1, rb.velocity.y), 3*Time.deltaTime);
                    direction = false;
                    spriteRenderer.flipX = true;
                }
            }
            if (Input.GetKey(KeyCode.W))
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(rb.velocity.x, MaxHorizontalSpeed), 2*Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(rb.velocity.x, -MaxHorizontalSpeed), 2*Time.deltaTime);
            }

            if (rb.velocity.x > MaxHorizontalSpeed)
            {
                rb.velocity = new Vector2(MaxHorizontalSpeed, rb.velocity.y);
            }
            if (rb.velocity.x < -MaxHorizontalSpeed)
            {
                rb.velocity = new Vector2(-MaxHorizontalSpeed, rb.velocity.y);
            }

            
            if(rb.velocity == Vector2.zero)
            {
                SubmarineMoving.enabled = false;
            }
            else
            {
                SubmarineMoving.enabled = true;
            }
            
        }
        if(PlayerManager.instance.state == PlayerManager.PlayerState.Dead)
        {
            Die();
        }
        if(HeldCivilian != null)
        {
            HeldCivilian.transform.position = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, 0);
        }
    }

    void Fire(){
        GameObject NewBullet = Instantiate(Bullet, new Vector3(transform.position.x - 4* (direction ? 1 : -1), transform.position.y, transform.position.z), Quaternion.identity);
        NewBullet.GetComponent<BulletLogic>().HorizontalSpeed = (direction ? 1 : -1) * ProjectileSpeed;
        NewBullet.transform.localScale = NewBullet.transform.localScale*(direction ? 1 : -1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemyBullet") && !dying)
        {
            if (PlayerManager.instance.state != PlayerManager.PlayerState.Dead)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
                dying = true;
                SubmarineMoving.enabled = true;
                rb.velocity = Vector2.zero;
                GetComponent<PolygonCollider2D>().isTrigger = true;
                anim.SetBool("Dead", true);
                StartCoroutine(DeathAnimation());
            }
        }
        print("PlayerLogic: " + (collision.transform.CompareTag("Enemy") && !dying).ToString());
         if (collision.transform.CompareTag("Enemy") && !dying)
        {
//            print(collision.name);
            //!collision.transform.gameObject.GetComponent<JellyFishLogic>().dyingProcess saving for later
            if (PlayerManager.instance.state != PlayerManager.PlayerState.Dead
                && ((collision.transform.gameObject.GetComponent<JellyFishLogic>() && !collision.transform.gameObject.GetComponent<JellyFishLogic>().dyingProcess)
                || (collision.transform.gameObject.GetComponent<MutantJellyFishLogic>() && !collision.transform.gameObject.GetComponent<MutantJellyFishLogic>().dyingProcess)) )
            { 
                spriteRenderer.flipX = !spriteRenderer.flipX;
                SubmarineMoving.enabled = true;
                dying = true;
                rb.velocity = Vector2.zero;
                GetComponent<PolygonCollider2D>().isTrigger = true;
                anim.SetBool("Dead", true);
                StartCoroutine(DeathAnimation());
            }

        }
        
            if (collision.transform.CompareTag("Civilian") && !collision.transform.gameObject.GetComponent<CivilianLogic>().isOnCeiling && HeldCivilian == null && !collision.transform.gameObject.GetComponent<CivilianLogic>().isOnCeiling)
        {
            HeldCivilian = collision.gameObject;
            HeldCivilian.transform.parent = transform;
            HeldCivilian.transform.position = new Vector3(transform.position.x, transform.position.y - transform.localScale.y*2, 0);
            JellyFishManager.instance.people.Remove(HeldCivilian);
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.transform.tag);
        if (collision.transform.CompareTag("Ceiling"))
        {
             if (HeldCivilian != null) 
            { 
                JellyFishManager.instance.people.Add(HeldCivilian);
                HeldCivilian.GetComponent<CivilianLogic>().isOnCeiling = true;
                HeldCivilian.transform.parent = null; 
                HeldCivilian.transform.position = transform.position;
                HeldCivilian = null;
                PlayerInfo.score+=500;
            }
        }
    }

    public void Die()
    {
        if(!spriteRenderer.enabled)
        {
        Vector3 direction = Vector2.Lerp(transform.position, Vector3.zero, Time.deltaTime*5);
        transform.position = direction;
        }else
        {
            rb.velocity = Vector2.zero;
        }
        if(Vector3.Distance(transform.position,  Vector3.zero) < 0.1)
        {
            SceneManager.LoadScene(1);
            if(SceneManager.GetActiveScene().name=="DeeperWater")
                SceneManager.LoadScene("DeeperWater");
        }
    }

    IEnumerator DeathAnimation()
    {
        FMODUnity.RuntimeManager.PlayOneShot(DeathEvent, transform.position);
        yield return new WaitForSeconds(1f);
        spriteRenderer.enabled = false;
        DeathParticles.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(3f);
        PlayerInfo.lives --;
        PlayerManager.instance.state = PlayerManager.PlayerState.Dead;
        spriteRenderer.enabled = false;
    }

    IEnumerator Delay(){
        yield return new WaitForSeconds(0.5f);
        fireCoolDown = false;
    }

    IEnumerator BombDelay()
    {
        BombFlash.GetComponent<UnityEngine.UI.Image>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        BombFlash.GetComponent<UnityEngine.UI.Image>().enabled = false;
        bombing = false;
    }

    bool WhirlPool()
    {
        WhirlPoolAnimation.GetComponent<UnityEngine.UI.Image>().enabled = true;
        WhirlPoolAnimation.GetComponent<Animator>().SetBool("WhirlPool", true);
        timeScale += Time.deltaTime;
        BackgroundManager.transform.position = new Vector2(transform.position.x, 0);
        if(whirlPoolLocation == 0)
        {
        whirlPoolLocation = SetWhirlPoolDirection();
        FMODUnity.RuntimeManager.PlayOneShot(Teleport, transform.position);
        }
        Vector2 Direction = Vector2.Lerp(transform.position, new Vector2(whirlPoolLocation, transform.position.y), timeScale/3);
        transform.position = Direction;
        if(Vector2.Distance(transform.position, new Vector2(whirlPoolLocation, transform.position.y)) < 0.1)
        {
            GetComponentInParent<PolygonCollider2D>().enabled = true;
            whirlPoolLocation = 0;
            timeScale = 0;
            WhirlPoolAnimation.GetComponent<UnityEngine.UI.Image>().enabled = false;
            WhirlPoolAnimation.GetComponent<Animator>().SetBool("WhirlPool", false);
            return false;
        }
        GetComponentInParent<PolygonCollider2D>().enabled = false;
        return true;
        
    }

    float SetWhirlPoolDirection()
    {
      
        whirlPoolLocation = (int) UnityEngine.Random.Range(-1, 2);
        float Placement = UnityEngine.Random.Range(0, MapSize.transform.lossyScale.x/2);
         WhirlPoolspeed = (Placement * whirlPoolLocation)/50f;
        return Placement * whirlPoolLocation;
        
    }

    IEnumerator EcologicalDeadZoneCountDown()
    {
        yield return new WaitForSeconds(180f);
        DeepZoneText.GetComponent<TextMeshProUGUI>().enabled = true;
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("DeeperWater");
    }

    void GameJover()
    {
        loserBG.SetActive(true);
        loseText.text = "GAME OVER";
        loseText.enabled = true;
        theMiniMap.SetActive(false);

        replayBox.SetActive(true);
        homeBox.SetActive(true);
        replayButton.SetActive(true);
        homeButton.SetActive(true);
    }
}
