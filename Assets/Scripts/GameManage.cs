using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManage : MonoBehaviour
{
    public static bool generate;
    public static bool fading;

    public GameObject key;
    public GameObject fade;

    GameObject[] enemies;
    int keyEnemy;
    bool keyGenerated;
    bool start;
    Vector2 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        start = true;
        keyGenerated = false;
        generate = false;
        fading = true;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        keyEnemy = Random.Range(0, enemies.Length);
        fade.GetComponent<Image>().color = new Color(0, 0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
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

        if (enemies[keyEnemy])
        {
            lastPos = enemies[keyEnemy].transform.position;
        }
        if (!enemies[keyEnemy] && !keyGenerated)
        {
            Instantiate(key, lastPos, Quaternion.identity);
            keyGenerated = true;
        }
    }
}
