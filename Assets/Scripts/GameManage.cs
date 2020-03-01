using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
    public static bool newRoom;

    public float fadeSpeed;

    public GameObject key;
    public GameObject cam;
    public GameObject player;
    public GameObject goal;
    public GameObject StartCanvas, PlayCanvas, GameOverCanvas;

    [Tooltip("This is the offset between rooms")]
    public float xOffset, yOffset;

    public static GameObject[] rooms;
    public static bool[] keyCollected;
    Vector2 currentRoom;
    public static bool moving;
    GameObject previousRoom;

    // Start is called before the first frame update
    void Start()
    {
        StartCanvas.SetActive(true);
        PlayCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        newRoom = false;

        currentRoom = new Vector2(0, 0);
        rooms = GameObject.FindGameObjectsWithTag("Room");
        
        keyCollected = new bool[rooms.Length];
        for(int i = 0; i < rooms.Length; i++)
        {
            keyCollected[i] = false;
            if (rooms[i].name != "Room00")
            {
                foreach (Transform game in rooms[i].transform)
                {
                    if (game.GetComponent<SpriteRenderer>())
                    {
                        game.GetComponent<SpriteRenderer>().color = new Color(game.GetComponent<SpriteRenderer>().color.r, game.GetComponent<SpriteRenderer>().color.g, game.GetComponent<SpriteRenderer>().color.b, 0);
                    }
                }
                rooms[i].SetActive(false);
            }
            else
            {
                previousRoom = rooms[i];
            }
        }
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if reaches goal, turn on game over canvas
        if (!goal || PlayerController.LP <= 0)
        {
            GameOverCanvas.SetActive(true);
        }

        //currently not in use
        #region Canvas Transition Fade in/out
        if (StartCanvas && Input.anyKeyDown)
        {
            StartCanvas.SetActive(false);
            PlayCanvas.SetActive(true);
        }
        if (StartCanvas && Input.touchCount>0)
        {
            StartCanvas.SetActive(false);
            PlayCanvas.SetActive(true);
        }
        #endregion

        for (int i = 0; i < keyCollected.Length; i++)
        {
            if (keyCollected[i])
            {
                foreach(Transform go in rooms[i].transform)
                {
                 if (go.tag == "Door")
                    {
                        go.GetComponent<SpriteRenderer>().enabled = false;
                        go.GetComponent<Collider2D>().isTrigger = true;
                    }
                }
            }
        }

        

        if (newRoom)
        {
            //check which room to fade in and move camera to
            float xdiff = player.transform.position.x - cam.transform.position.x;
            float ydiff = player.transform.position.y - cam.transform.position.y;

            if (Mathf.Abs(xdiff) < Mathf.Abs(ydiff))
            {
                currentRoom += new Vector2(1 * Mathf.Sign(ydiff), 0);
                for(int i = 0; i < rooms.Length; i++)
                {
                    if (rooms[i].name == "Room" + currentRoom.x + currentRoom.y)
                    {
                        rooms[i].SetActive(true);
                        //previousRoom.SetActive(false);
                        //previousRoom = rooms[i];
                    }
                }
                moving = true;
                newRoom = false;
            }
            else
            {
                currentRoom += new Vector2(0, 1 * Mathf.Sign(xdiff));
                for (int i = 0; i < rooms.Length; i++)
                {
                    if (rooms[i].name == "Room" + currentRoom.x + currentRoom.y)
                    {
                        rooms[i].SetActive(true);
                        //previousRoom.SetActive(false);
                        //previousRoom = rooms[i];
                    }
                }
                moving = true;
                newRoom = false;
            }
            
        }

        if (moving)
        {

            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i].name == "Room" + currentRoom.x + currentRoom.y)
                {
                    foreach (Transform go in rooms[i].transform)
                    {
                        if (go.CompareTag("Door"))
                        {
                            go.GetComponent<SpriteRenderer>().enabled = false;
                            go.GetComponent<Collider2D>().isTrigger = true;
                        }
                    }
                    foreach (Transform game in rooms[i].transform)
                    {
                        if (game.GetComponent<SpriteRenderer>())
                        {
                            if (game.GetComponent<SpriteRenderer>().color.a < 1)
                            {
                                game.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
                            }

                        }
                    }

                }
            }

            //move camera to the center of the new room
            //currentRoom.x and currentRoom.y are put in the opposite position because for me it's easier to read if the first number represents y index instead of x index of a room
            Vector3 targetPos = new Vector3(currentRoom.y * xOffset, currentRoom.x * yOffset, cam.transform.position.z);

            cam.transform.position = Vector3.MoveTowards(cam.transform.position, targetPos, 10f * Time.deltaTime);
            if (cam.transform.position == targetPos)
            {
                for (int i = 0; i < rooms.Length; i++)
                {
                    if (rooms[i].name == "Room" + currentRoom.x + currentRoom.y)
                    {
                        previousRoom.SetActive(false);
                        previousRoom = rooms[i];
                    }
                }
                moving = false;
            }
        }
        else
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                if (rooms[i].name == "Room" + currentRoom.x + currentRoom.y)
                {
                    foreach (Transform go in rooms[i].transform)
                    {
                        if (go.CompareTag("Door") && !keyCollected[i])
                        {
                            go.GetComponent<SpriteRenderer>().enabled = true;
                            go.GetComponent<Collider2D>().isTrigger = false;
                        }
                    }
                }
            }
        }
        
    }
}
