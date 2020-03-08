using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulletMover : MonoBehaviour
{
    public float moveSpeed, waveSize;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Rigidbody2D>().velocity = transform.rotation * Vector2.up * speed;
        GetComponent<Rigidbody2D>().velocity = transform.rotation * new Vector2(Mathf.Sin(Time.time * 4f) * waveSize, 1) * moveSpeed;
        Debug.Log(Mathf.Sin(Time.time));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Door"))
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
