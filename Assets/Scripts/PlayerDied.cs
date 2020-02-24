using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This plays the dying sound
public class PlayerDied : MonoBehaviour
{
    public GameObject player;
    public AudioClip dieSound;

    private AudioSource Die;
    
    // Start is called before the first frame update
    void Start(){
        Die = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.LP == 0){
            Die.PlayOneShot(dieSound);
        }
    }
}
