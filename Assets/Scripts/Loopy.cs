using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loopy : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Camera;
    [SerializeField] GameObject miniCamera;
    public float loopSpeed = 2f;
    public float bgwidth;
    public float bgwidth2;
    public float blackbgwidth;
    private Transform bg1;
    private Transform bg2;
    private Transform bg3;
    private Transform bg4;
    // ajsjoitowsjtsijtnsjtonst helloso0ostosotso
    // HELLO
    [SerializeField] PlayerLogic pl;

    private Transform mbg1; // m stands for mini, 1 to 3 is from left to right
    private Transform mbg2;
    private Transform mbg3;
    private Transform mbg4;

    public GameObject blackBG;

    public GameObject bgStill;
    public GameObject bgStill2;

    public float bgspeed;


    // Start is called before the first frame update
    void Start()
    {
        //blackBG.SetActive(false);
        //bgStill.SetActive(false);
        pl = GetComponent<PlayerLogic>();
        bg1 = transform.GetChild(0);
        bg2 = transform.GetChild(1);
        bg3 = transform.GetChild(2);
        bg4 = transform.GetChild(3);

        mbg1 = transform.GetChild(4);
        mbg2 = transform.GetChild(5);
        mbg3 = transform.GetChild(6);
        mbg4 = transform.GetChild(7);

        //blackBG = transform.GetChild(8);

        bgwidth = bg1.GetComponent<SpriteRenderer>().bounds.size.x;
        bgwidth2 = mbg1.GetComponent<SpriteRenderer>().bounds.size.x;
        //blackbgwidth = blackBG.GetComponent<SpriteRenderer>().bounds.size.x;

        bg3.position = new Vector3(bg2.position.x + (bgwidth), bg2.position.y, bg2.position.z);
        bg4.position = new Vector3(bg3.position.x + (bgwidth), bg2.position.y, bg2.position.z);
        bg1.position = new Vector3(bg2.position.x - (bgwidth), bg2.position.y, bg2.position.z);

        mbg3.position = new Vector3(mbg2.position.x + (bgwidth2 - 2), mbg2.position.y, mbg2.position.z);
        mbg4.position = new Vector3(mbg3.position.x + (bgwidth2 - 2), mbg2.position.y, mbg2.position.z);
        mbg1.position = new Vector3(mbg2.position.x - (bgwidth2 - 2), mbg2.position.y, mbg2.position.z);
        Debug.Log("right");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (PlayerManager.instance.state == PlayerManager.PlayerState.Dead)
        {
            bgStill.SetActive(true);
            bgStill2.SetActive(true);
        }
        */
        loopSpeed = bgspeed*Player.GetComponent<Rigidbody2D>().velocity.x;
        bg1.position += Vector3.left * loopSpeed * Time.deltaTime;
        bg2.position += Vector3.left * loopSpeed * Time.deltaTime;
        bg3.position += Vector3.left * loopSpeed * Time.deltaTime;
        bg4.position += Vector3.left * loopSpeed * Time.deltaTime;

        mbg1.position += Vector3.left * loopSpeed * Time.deltaTime;
        mbg2.position += Vector3.left * loopSpeed * Time.deltaTime;
        mbg3.position += Vector3.left * loopSpeed * Time.deltaTime;
        mbg4.position += Vector3.left * loopSpeed * Time.deltaTime;

        //blackBG.transform.position += Vector3.left * loopSpeed * Time.deltaTime;


        if (loopSpeed > 0 && bg4.position.x <= Camera.transform.position.x)
        {
            bg1.position = new Vector3(bg4.position.x + bgwidth, bg1.position.y, bg1.position.z);

            Transform tempVar = bg1;
            bg1 = bg2;
            bg2 = bg3;
            bg3 = bg4;
            bg4 = tempVar;

        }

        if (loopSpeed > 0 && (mbg4.position.x + (bgwidth2 - 2) / 2) <= miniCamera.transform.position.x + (bgwidth2 - 2) * 1.5)
        {
            mbg1.position = new Vector3(mbg4.position.x + (bgwidth2 - 2), mbg1.position.y, mbg1.position.z);

            Transform tempVar = mbg1;
            mbg1 = mbg2;
            mbg2 = mbg3;
            mbg3 = mbg4;
            mbg4 = tempVar;
        }

        if (loopSpeed < 0 && bg1.position.x >= Camera.transform.position.x)
        {
            bg4.position = new Vector3(bg1.position.x - bgwidth, bg1.position.y, bg1.position.z);

            Transform tempVar = bg1;
            bg1 = bg2;
            bg2 = bg3;
            bg3 = bg4;
            bg4 = tempVar;
        }

        if (loopSpeed < 0 && (mbg1.position.x - (bgwidth2 - 2) / 2) >= miniCamera.transform.position.x - (bgwidth2 - 2) * 1.5)
        {
            mbg4.position = new Vector3(mbg1.position.x - (bgwidth2 - 2), mbg1.position.y, mbg1.position.z);

            Transform tempVar = mbg1;
            mbg1 = mbg2;
            mbg2 = mbg3;
            mbg3 = mbg4;
            mbg4 = tempVar;
        }

    }

}
