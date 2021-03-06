﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool onLaptop;
    public float speed;
    public GameObject[] HP;
    public GameObject Bullet;
    public Transform firePoint;
    public float fireRate;

    public FloatingJoystick leftJS, rightJS;

    AudioSource playerSource;

    public static int LP;

    bool invincible;
    SpriteRenderer sr;
    Collider2D c;
    Rigidbody2D rb;

    //animation
    Animator anim;
    SpriteRenderer characterSprite;

    // Start is called before the first frame update
    void Start()
    {
        invincible = false;
        LP = 3;
        HP[0].SetActive(true);
        HP[1].SetActive(true);
        HP[2].SetActive(true);
        rb = GetComponent<Rigidbody2D>();
        c = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        InvokeRepeating("Shoot", 0, fireRate);

        playerSource = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();
        characterSprite = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Fire a bullet
    /// </summary>
    void Shoot()
    {
		//shoot in laptop
		if (onLaptop)
		{
            if (Input.GetButton("Jump"))
		    {
			    Instantiate(Bullet, firePoint.position, transform.rotation);
		    }
		}

		//shoot in phone
		if (!onLaptop)
		{
            if (rightJS.Horizontal != 0 || rightJS.Vertical != 0)
		    {
			    Instantiate(Bullet, firePoint.position, transform.rotation);
		    }
		}
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Time.deltaTime > 0)
        {
			if (onLaptop)
			{
                //Disable joysticks
                leftJS.gameObject.SetActive(false);
                rightJS.gameObject.SetActive(false);

                //Player Movement in Laptop
                float x = Input.GetAxisRaw("Horizontal");
			    float y = Input.GetAxisRaw("Vertical");
                rb.velocity = new Vector2(x * speed, y * speed);

                //Player Rotation in Laptop
                Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.up = (mouseScreenPosition - (Vector2)transform.position).normalized;

                //animation
                if(y > 0)
                {
                    anim.SetBool("isUp", true);
                } else {
                    anim.SetBool("isUp", false);
                }

                if(y < 0)
                {
                    anim.SetBool("isDown", true);
                } else
                {
                    anim.SetBool("isDown", false);
                }

                if(Mathf.Abs(x) > 0)
                {
                    anim.SetBool("isHorizontal", true);
                } else
                {
                    anim.SetBool("isHorizontal", false);
                }

                if(x >= 0)
                {
                    characterSprite.flipX = false;
                } else
                {
                    characterSprite.flipX = true;
                }
            }

			if (!onLaptop)
			{
                //Player Movement on phone
                float x = leftJS.Horizontal;
                float y = leftJS.Vertical;
                rb.velocity = new Vector2(x * speed, y * speed);

                //Player Rotation on phone
                if (rightJS.Horizontal != 0 && rightJS.Vertical != 0)
                {
                    transform.up = rightJS.Direction.normalized;
                }
            }
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If hit by enemy bullet, destroy bullet and reduce health
        if (collision.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
            if (!invincible)
            {
                LP -= 1;
                Hurt();
            }
            
        }
        //if hit by enemy, reduce health
        if (collision.CompareTag("Enemy"))
        {
            if (!invincible)
            {
                LP -= 1;
                Hurt();
            }
        }

        //if collect key, set bool to true
        if (collision.CompareTag("Key"))
        {
            int n = System.Array.IndexOf(GameManage.rooms, collision.gameObject.transform.parent.gameObject);
            GameManage.keyCollected[n] = true;
            Destroy(collision.gameObject);
        }

        //for testing, opens game over canvas in GameManager script
        if (collision.gameObject.name == "Goal")
        {
            Destroy(collision.gameObject);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
	{
        if (collision.CompareTag("Enemy"))
        {
            if (!invincible)
            {
                LP -= 1;
                Hurt();
            }
        }
    }

    /// <summary>
    /// Reduce HP if Hurt
    /// </summary>
    void Hurt()
    {
        HP[LP].SetActive(false);
        if (LP == 0)
        {
            Destroy(gameObject);
        }

        StopAllCoroutines();
        sr.color = new Color(255, 255, 255, 1);
        c.enabled = true;
        //StartCoroutine("NotCollide");
        StartCoroutine("Blink");
    }

    /*
    /// <summary>
    /// A Period of Invincible Time After Hurt
    /// </summary>
    /// <returns></returns>
    IEnumerator NotCollide()
    {
        c.enabled = false;
        yield return new WaitForSeconds(.5f);
        c.enabled = true;
        yield break;
    }
    */

    /// <summary>
    /// Blink Animation When Hurt
    /// </summary>
    /// <returns></returns>
    IEnumerator Blink()
    {
        invincible = true;
        for (int i = 0; i < 3; i++)
        {
            sr.color = new Color(255, 255, 255, 0);
            yield return new WaitForSeconds(.1f);
            sr.color = new Color(255, 255, 255, 1);
            yield return new WaitForSeconds(.1f);
        }
        invincible = false;
    }
}
