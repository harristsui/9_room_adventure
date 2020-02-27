using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This plays the dying sound
public class PlayerDied : MonoBehaviour
{
    public GameObject player;
    public AudioClip dieSound;

    AudioSource aud;
    bool died;

    // Start is called before the first frame update
    void Start()
    {
        aud = GetComponent<AudioSource>();
        died = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.LP == 0 && !died)
        {
            aud.PlayOneShot(dieSound);
            died = true;
        }
    }
}
