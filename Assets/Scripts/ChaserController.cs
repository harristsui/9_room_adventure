using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserController : MonoBehaviour
{
    public float speed;
    public float HP;

    GameObject Player;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManage.fading)
        {
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
            rb.velocity = speed * (Player.transform.position - transform.position);
            transform.up = ((Vector2)Player.transform.position - (Vector2)transform.position).normalized;
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
