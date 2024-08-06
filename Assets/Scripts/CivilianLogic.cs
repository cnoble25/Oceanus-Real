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
//using static JellyFishManager;

public class CivilianLogic : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float CivilianVerticalSpeed;
    [SerializeField] public GameObject DeathParticles;

    [SerializeField] GameObject DeepZoneText;

    [SerializeField] GameObject WhirlPool;

    Animator anim;


    public bool isOnCeiling = false;

    bool isDying;

    public GameObject Holder;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(gameObject!=null) JellyFishManager.instance.people.Add(gameObject);

        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Holder == null && !isOnCeiling) {rb.velocity = new Vector3(0, CivilianVerticalSpeed, 0);}
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ceiling"))
        {
            isOnCeiling = true;
            rb.velocity = Vector3.zero;
        }
        if (collision.transform.CompareTag("Bullet") && !isDying && Holder == null)
        {
            Destroy(collision.gameObject);
            isDying = true;
            anim.SetBool("Dying", true);
            StopAllCoroutines();
            //DeathParticles.GetComponent<ParticleSystem>().Play();
            print("IM DEAD");
            StartCoroutine(DeathAnimation());
        }
    }

    IEnumerator DeathAnimation()
    {
        JellyFishManager.instance.people.Remove(gameObject);
        yield return new WaitForSeconds(1f);
        GetComponent<SpriteRenderer>().enabled = false;
        DeathParticles.GetComponent<ParticleSystem>().Play();
        print("explodeeee");
        yield return new WaitForSeconds(3f);
        int count = JellyFishManager.instance.people.Count;
        if (count == 1)
        {
            DeepZoneText.GetComponent<TextMeshProUGUI>().enabled = true;
            yield return new WaitForSeconds(5f);
            WhirlPool.GetComponent<UnityEngine.UI.Image>().enabled = true;
            WhirlPool.GetComponent<Animator>().SetBool("WhirlPool", true);
            yield return new WaitForSeconds(0.5f);
            JellyFishManager.instance.JellyFish.Clear();
            MainMenu.mm.GoDeeper();
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
