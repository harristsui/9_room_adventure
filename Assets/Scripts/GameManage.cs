using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
    public static bool newRoom;
    public static GameObject nextRoom;
    public static GameObject lastRoom;
    public static bool moving;
    public static Vector2 camTarget;
    public float fadeSpeed;

    public GameObject key;
    public GameObject cam;
    public GameObject player;
    public GameObject goal;
    public GameObject StartCanvas, PlayCanvas, GameOverCanvas;

    public Vector2 doorColliderOffset;

    [Tooltip("This is the offset between rooms")]
    public float xOffset, yOffset;

    GameObject[] regularRooms, LRooms, largeRooms;
    public static List<GameObject> rooms;
    public static bool[] keyCollected;
    Vector2 currentRoom;
    
    GameObject previousRoom;

    float upDist, downDist, rightDist, leftDist;

    // Start is called before the first frame update
    void Start()
    {
        StartCanvas.SetActive(true);
        PlayCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        newRoom = false;

        currentRoom = new Vector2(0, 0);

        regularRooms = GameObject.FindGameObjectsWithTag("Room");
        LRooms = GameObject.FindGameObjectsWithTag("LRoom");
        largeRooms = GameObject.FindGameObjectsWithTag("LargeRoom");
        rooms = new List<GameObject>();
        rooms.AddRange(regularRooms);
        rooms.AddRange(LRooms);
        rooms.AddRange(largeRooms);
        
        keyCollected = new bool[rooms.Count];
        for(int i = 0; i < rooms.Count; i++)
        {
            keyCollected[i] = false;
            if (rooms[i].name != "Room00")
            {
                //foreach (Transform game in rooms[i].transform)
                //{
                //    if (game.GetComponent<SpriteRenderer>())
                //    {
                //        game.GetComponent<SpriteRenderer>().color = new Color(game.GetComponent<SpriteRenderer>().color.r, game.GetComponent<SpriteRenderer>().color.g, game.GetComponent<SpriteRenderer>().color.b, 0);
                //    }
                //}
                rooms[i].SetActive(false);
            }
            else
            {
                previousRoom = rooms[i];
            }
        }
        moving = false;

        #region Geting default distances between the camera and the walls
        int detectable = LayerMask.GetMask("RaycastDetectable");
        //Vector3 detectOrigin = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);

        //upward raycast
        RaycastHit2D upHit = Physics2D.Raycast(cam.transform.position, Vector2.up, Mathf.Infinity, detectable);
        upDist = upHit.distance;
        //downward raycast
        RaycastHit2D downHit = Physics2D.Raycast(cam.transform.position, -Vector2.up, Mathf.Infinity, detectable);
        downDist = downHit.distance;
        //right raycast
        RaycastHit2D rightHit = Physics2D.Raycast(cam.transform.position, Vector2.right, Mathf.Infinity, detectable);
        rightDist = rightHit.distance;
        //left raycast
        RaycastHit2D leftHit = Physics2D.Raycast(cam.transform.position, -Vector2.right, Mathf.Infinity, detectable);
        leftDist = leftHit.distance;
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

        #region Raycasting from camera
        /*
        if (!moving)
        {
            int detectable = LayerMask.GetMask("RaycastDetectable");

            //Vector3 detectOrigin = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);

            //upward raycast
            RaycastHit2D upHit = Physics2D.Raycast(cam.transform.position, Vector2.up, Mathf.Infinity, detectable);
            Ray2D upRay = new Ray2D(cam.transform.position, Vector2.up);
            Debug.DrawRay(cam.transform.position, Vector2.up * upHit.distance, Color.red);
            //downward raycast
            RaycastHit2D downHit = Physics2D.Raycast(cam.transform.position, -Vector2.up, Mathf.Infinity, detectable);
            Ray2D downRay = new Ray2D(cam.transform.position, -Vector2.up);
            Debug.DrawRay(cam.transform.position, -Vector2.up * downHit.distance, Color.red);
            //right raycast
            RaycastHit2D rightHit = Physics2D.Raycast(cam.transform.position, Vector2.right, Mathf.Infinity, detectable);
            Ray2D rightRay = new Ray2D(cam.transform.position, Vector2.right);
            Debug.DrawRay(cam.transform.position, Vector2.right * rightHit.distance, Color.red);
            //left raycast
            RaycastHit2D leftHit = Physics2D.Raycast(cam.transform.position, -Vector2.right, Mathf.Infinity, detectable);
            Ray2D leftRay = new Ray2D(cam.transform.position, -Vector2.right);
            Debug.DrawRay(cam.transform.position, -Vector2.right * leftHit.distance, Color.red);

            Debug.Log("up: " + upHit.distance + ", down: " + downHit.distance + ", left: " + leftHit.distance + ", right: " + rightHit.distance);
        }
        */
        #endregion


        //if reaches goal, turn on game over canvas
        if (!goal || PlayerController.LP <= 0)
        {
            GameOverCanvas.SetActive(true);
        }

        //Enter the game
        #region Canvas Transition
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
                foreach (Transform go in rooms[i].transform)
                {
                    if (go.tag == "Door")
                    {
                        go.GetComponent<SpriteRenderer>().enabled = false;
                        go.GetComponent<Collider2D>().isTrigger = true;
                        go.GetComponent<Collider2D>().offset = doorColliderOffset;
                    }
                }
            }
        }



        if (newRoom)
        {
            /*
            //check which room to fade in and move camera to
            float xdiff = player.transform.position.x - cam.transform.position.x;
            float ydiff = player.transform.position.y - cam.transform.position.y;

            if (Mathf.Abs(xdiff) < Mathf.Abs(ydiff))
            {
                currentRoom += new Vector2(1 * Mathf.Sign(ydiff), 0);
                for(int i = 0; i < rooms.Count; i++)
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
                for (int i = 0; i < rooms.Count; i++)
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
            */

        }

        if (moving)
        {

            //for (int i = 0; i < rooms.Count; i++)
            //{
            //    if (rooms[i].name == "Room" + currentRoom.x + currentRoom.y)
            //    {
            //        foreach (Transform go in rooms[i].transform)
            //        {
            //            if (go.CompareTag("Door"))
            //            {
            //                go.GetComponent<DoorController>().MoveCamTo
            //            }
            //        }


            //    }
            //}
            nextRoom.SetActive(true);
            //lastRoom.GetComponent<Room>().fadeOut = true;
            //move camera to the center of the new room
            //currentRoom.x and currentRoom.y are put in the opposite position because for me it's easier to read if the first number represents y index instead of x index of a room
            Vector3 targetPos = new Vector3(camTarget.x, camTarget.y, cam.transform.position.z);
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, targetPos, 10f * Time.deltaTime);

            if (cam.transform.position == targetPos)
            {
                moving = false;
            }
        }
        else
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].name == "Room" + currentRoom.x + currentRoom.y)
                {
                    foreach (Transform go in rooms[i].transform)
                    {
                        if (go.CompareTag("Door") && !keyCollected[i])
                        {
                            go.GetComponent<SpriteRenderer>().enabled = true;
                            go.GetComponent<Collider2D>().isTrigger = false;
                            go.GetComponent<Collider2D>().offset = new Vector2(0, 0);
                        }
                    }
                }
            }
        }
        
    }
}
