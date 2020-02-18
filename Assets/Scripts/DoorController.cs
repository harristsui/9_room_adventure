using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject keyUI;

    SpriteRenderer sr;
    Collider2D c;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        c = GetComponent<Collider2D>();
        sr.enabled = true;
        c.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (keyUI.activeSelf)
        {
            sr.enabled = false;
            c.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManage.generate = true;
        }
    }
}
