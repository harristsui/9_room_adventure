using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
    public static bool generate;
    public static bool fading;

    public static bool newRoom;

    public GameObject key;
    public GameObject fade;
    public GameObject cam;
    public GameObject player;

    [Tooltip("This is the offset between rooms")]
    public float xOffset, yOffset;

    GameObject[] enemies;
    int keyEnemy;
    bool keyGenerated;
    bool start;
    Vector2 lastPos;

    public static GameObject[] rooms;
    public static bool[] keyCollected;
    Vector2 currentRoom;
    bool moving;

    // Start is called before the first frame update
    void Start()
    {
        //currently set to false
        //fading = true;
        //start = true;
        fading = false;
        start = false;

        keyGenerated = false;
        generate = false;

        newRoom = false;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        keyEnemy = Random.Range(0, enemies.Length);
        fade.GetComponent<Image>().color = new Color(0, 0, 0, 1);

        currentRoom = new Vector2(0, 0);
        rooms = GameObject.FindGameObjectsWithTag("Room");
        
        keyCollected = new bool[rooms.Length];
        for(int i = 0; i < rooms.Length; i++)
        {
            keyCollected[i] = false;
        }
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        //currently not in use
        #region Scene Transition Fade in/out
        /*
        if (start)
        {
            fade.GetComponent<Image>().color -= new Color(0, 0, 0, Time.deltaTime);
            fading = true;
            if (fade.GetComponent<Image>().color.a <= 0)
            {
                start = false;
                fading = false;
            }
        }
        if (generate)
        {
            fade.GetComponent<Image>().color += new Color(0, 0, 0, Time.deltaTime);
            fading = true;
            if (fade.GetComponent<Image>().color.a >= 1)
            {
                SceneManager.LoadScene("Test");
            }
        }
        */
        #endregion

        #region Let Random Enemy Drop Key
        if (enemies[keyEnemy])
        {
            lastPos = enemies[keyEnemy].transform.position;
        }
        if (!enemies[keyEnemy] && !keyGenerated)
        {
            Instantiate(key, lastPos, Quaternion.identity, GameObject.Find("Room" + currentRoom.x + currentRoom.y).transform);
            keyGenerated = true;
        }
        #endregion

        for(int i = 0; i < keyCollected.Length; i++)
        {
            if (keyCollected[i])
            {
                foreach(Transform go in rooms[i].transform)
                {
                    if (go.name == "Door")
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
                currentRoom += new Vector2(0, 1 * Mathf.Sign(ydiff));
                moving = true;
                newRoom = false;
            }
            else
            {
                currentRoom += new Vector2(1 * Mathf.Sign(xdiff), 0);
                moving = true;
                newRoom = false;
            }          
        }

        if (moving)
        {
            //move camera to the center of the new room
            Vector3 targetPos = new Vector3(currentRoom.x * xOffset, currentRoom.y * yOffset, cam.transform.position.z);
            Debug.Log(targetPos);
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, targetPos, 10f * Time.deltaTime);
            if (cam.transform.position == targetPos)
            {
                moving = false;
            }
        }
        
    }
}
