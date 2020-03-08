using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDoorController : MonoBehaviour
{
    public GameObject WinCanvas;

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

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            WinCanvas.SetActive(true);
        }
    }
}
