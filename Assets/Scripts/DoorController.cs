using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject ConnectedRoom;
    public Vector2 MoveCamTo;

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
        if (collision.CompareTag("Player") && !GameManage.moving)
        {
            GameManage.moving = true;
            GameManage.nextRoom = ConnectedRoom;
            //GameManage.lastRoom = transform.parent.gameObject;
            GameManage.camTarget = MoveCamTo;
            transform.GetComponentInParent<Room>().fadeOut = true;
        }
    }
}
