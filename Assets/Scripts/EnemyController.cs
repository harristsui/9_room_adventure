using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int nBullets;
    public float calmTime, chaseTime;
    public float speed;
    public float forwardDist;
    public float HP;
    public GameObject bullet;

    GameObject Player;
    GameObject[] bullets;
    int chase;
    Rigidbody2D rb;
    float timer, startTime;
    Vector2 oldPos;
    float targetY;
    bool fired;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        bullets = new GameObject[nBullets];
        chase = 0;
        rb = GetComponent<Rigidbody2D>();
        timer = 0;
        oldPos = transform.position;
        fired = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManage.fading && Player)
        {
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
            timer += Time.deltaTime;
            targetY = Player.transform.position.y;

            if ((timer - startTime) >= chaseTime && chase == 0)
            {
                startTime = timer;
                chase = 1;
            }
            if ((timer - startTime) >= calmTime && chase == 1)
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
            Instantiate(bullet, transform.position, transform.rotation);
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
