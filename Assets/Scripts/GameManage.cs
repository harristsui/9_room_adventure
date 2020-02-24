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
    public Vector3 xOffset, yOffset;

    GameObject[] enemies;
    int keyEnemy;
    bool keyGenerated;
    bool start;
    Vector2 lastPos;
    Vector2 roomIndex;

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

        roomIndex = new Vector2(0, 0);

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        keyEnemy = Random.Range(0, enemies.Length);
        fade.GetComponent<Image>().color = new Color(0, 0, 0, 1);
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
            Instantiate(key, lastPos, Quaternion.identity);
            keyGenerated = true;
        }
        #endregion

        if (newRoom)
        {
            //check which room to fade in and move camera to
            /*
            float xdiff = player.transform.position.x - cam.transform.position.x;
            float ydiff = player.transform.position.y - cam.transform.position.y;
            if (Mathf.Abs(xdiff) < Mathf.Abs(ydiff))
            {

            }
            else
            {

            }
            */
            
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, xOffset, 10f * Time.deltaTime);
            if (cam.transform.position == xOffset)
            {
                newRoom = false;
            }
        }
    }
}
