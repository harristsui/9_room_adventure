using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum roomType { Small, Long, Big};
    public roomType thisRoomType;
    Bounds cameraMoveBound;

    public bool fadeIn, fadeOut;
    float fadeSpeed = 3f;
    Transform player;
    GameObject cam;
    Vector3 targetPos;

    SpriteRenderer[] sr;

    // Start is called before the first frame update
    void Start()
    {
        fadeIn = true;
        fadeOut = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<SpriteRenderer>())
            {
                child.GetComponent<SpriteRenderer>().color = new Color(child.GetComponent<SpriteRenderer>().color.r, child.GetComponent<SpriteRenderer>().color.g, child.GetComponent<SpriteRenderer>().color.b, 0);
            }
        }
        sr = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        fadeIn = true;
        fadeOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region Fade In/Out
        if (fadeIn)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<SpriteRenderer>())
                {
                    if (child.gameObject.GetComponent<SpriteRenderer>().color.a < 1)
                    {
                        child.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
                    }
                    if (child.gameObject.GetComponent<SpriteRenderer>().color.a >= 1)
                    {
                        child.GetComponent<SpriteRenderer>().color = new Color(child.GetComponent<SpriteRenderer>().color.r, child.GetComponent<SpriteRenderer>().color.g, child.GetComponent<SpriteRenderer>().color.b, 1);
                        fadeIn = false;
                    }
                }
            }
        }
        if (fadeOut)
        {
            foreach (Transform child in transform)
            {
                if (child.gameObject.GetComponent<SpriteRenderer>())
                {
                    if(child.gameObject.GetComponent<SpriteRenderer>().color.a > 0)
                    {
                        child.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, Time.deltaTime * fadeSpeed);
                    }
                    if(child.gameObject.GetComponent<SpriteRenderer>().color.a <= 0)
                    {
                        fadeOut = false;
                        gameObject.SetActive(false);
                    }
                }
            }
        }
        #endregion

        #region Camera Movement for Different Rooms
        if (!GameManage.moving)
        {
            if (thisRoomType == roomType.Long)
            {
                cameraMoveBound = new Bounds(new Vector3(9.5f + transform.position.x, 4.5f + transform.position.y, 0), new Vector3(19f, 9f, 0));
                if (player.position.y < cameraMoveBound.min.y)
                {
                    targetPos.y = cameraMoveBound.min.y;
                }
                else if (player.position.y > cameraMoveBound.max.y)
                {
                    targetPos.y = cameraMoveBound.max.y;
                }
                else
                {
                    targetPos.y = player.position.y;
                }
                targetPos.x = cameraMoveBound.min.x;
                targetPos.z = cam.transform.position.z;
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, targetPos, 10f * Time.deltaTime);
            }

            if (thisRoomType == roomType.Big)
            {
                cameraMoveBound = new Bounds(new Vector3(-9.5f + transform.position.x, 4.5f + transform.position.y, 0), new Vector3(19f, 9f, 0));
                //if (cameraMoveBound.Contains(player.position))
                //{
                //    targetPos.x = player.position.x;
                //    targetPos.y = player.position.y;
                //}
                if (player.position.x <= cameraMoveBound.min.x)
                {
                    targetPos.x = cameraMoveBound.min.x;
                }else if (player.position.x >= cameraMoveBound.max.x)
                {
                    targetPos.x = cameraMoveBound.max.x;
                }
                else
                {
                    targetPos.x = player.position.x;
                }
                if (player.position.y <= cameraMoveBound.min.y)
                {
                    targetPos.y = cameraMoveBound.min.y;
                }else if (player.position.y >= cameraMoveBound.max.y)
                {
                    targetPos.y = cameraMoveBound.max.y;
                }
                else
                {
                    targetPos.y = player.position.y;
                }
                targetPos.z = cam.transform.position.z;
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, targetPos, 10f * Time.deltaTime);
            }
        }
        #endregion

    }
}
