using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject[] HP;
    public GameObject Bullet;
    public Transform firePoint;
    public float fireRate;
    public GameObject keyUI;

    public static int LP;

    SpriteRenderer sr;
    Collider2D c;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        LP = 3;
        HP[0].SetActive(true);
        HP[1].SetActive(true);
        HP[2].SetActive(true);
        keyUI.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        c = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        InvokeRepeating("Shoot", 0, fireRate);
    }

    /// <summary>
    /// Fire a bullet
    /// </summary>
    void Shoot()
    {
        if (Input.GetButton("Jump"))
        {
            Instantiate(Bullet, firePoint.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManage.fading)
        {
            //Player Movement
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(x * speed, y * speed);

            //Player Rotation
            Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.up = (mouseScreenPosition - (Vector2)transform.position).normalized;
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If hit by enemy bullet, destroy bullet and reduce health
        if (collision.CompareTag("EnemyBullet"))
        {
            LP -= 1;
            Destroy(collision.gameObject);

            Hurt();
        }
        //if hit by enemy, reduce health
        if (collision.CompareTag("Enemy"))
        {
            LP -= 1;
            Hurt();
            Handheld.Vibrate();
        }

        //if collect key, show key UI
        if (collision.CompareTag("Key"))
        {
            keyUI.SetActive(true);
            Destroy(collision.gameObject);
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
        StartCoroutine("NotCollide");
        StartCoroutine("Blink");
    }

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

    /// <summary>
    /// Blink Animation When Hurt
    /// </summary>
    /// <returns></returns>
    IEnumerator Blink()
    {
        for (int i = 0; i < 3; i++)
        {
            sr.color = new Color(255, 255, 255, 0);
            yield return new WaitForSeconds(.1f);
            sr.color = new Color(255, 255, 255, 1);
            yield return new WaitForSeconds(.1f);
        }
    }
}
