using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject bossBullet;
    public GameObject winCanvas;
    public int nBullets;
    public float speed;
    public float forwardDist;
    public float HP;

    GameObject Player;
    Transform firePoint;
    int chase;
    Rigidbody2D rb;
    float timer, startTime;
    bool fired;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        chase = 0;
        rb = GetComponent<Rigidbody2D>();
        timer = 0;
        fired = false;
        firePoint = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player)
        {
            if (HP <= 0)
            {
                winCanvas.SetActive(true);
                Destroy(gameObject);
            }
            timer += Time.deltaTime;

            if ((timer - startTime) >= Random.Range(1, 3) && chase == 0)
            {
                startTime = timer;
                chase = 1;
            }
            if ((timer - startTime) >= Random.Range(2, 4) && chase == 1)
            {
                startTime = timer;
                chase = 0;
            }
            Behavior();
        }

    }

    void Behavior()
    {
        switch (chase)
        {
            case 0:
                rb.velocity = speed * (Player.transform.position - transform.position);
                transform.up = ((Vector2)Player.transform.position - (Vector2)transform.position).normalized;
                fired = false;
                break;

            case 1:
                rb.velocity = Vector2.zero;
                transform.up = ((Vector2)Player.transform.position - (Vector2)transform.position).normalized;
                if (fired == false)
                {
                    StartCoroutine("FireBullets");
                }
                fired = true;
                break;
        }
    }

    IEnumerator FireBullets()
    {
        for (int i = 0; i < nBullets; i++)
        {
            yield return new WaitForSeconds(.2f);
            Instantiate(bossBullet, firePoint.position, transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);
            HP--;
        }
    }
}
