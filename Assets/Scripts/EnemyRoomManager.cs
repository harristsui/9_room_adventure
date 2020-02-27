using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoomManager : MonoBehaviour
{
    public GameObject key;

    int keyEnemy;
    List<GameObject> enemies;
    Vector2 lastPos;
    bool keyGenerated;

    // Start is called before the first frame update
    void Start()
    {
        keyGenerated = false;
        enemies = new List<GameObject>();
        foreach (Transform enemy in transform)
        {
            if (enemy.tag == "Enemy")
            {
                if (!enemies.Contains(enemy.gameObject))
                {
                    enemies.Add(enemy.gameObject);
                }
            }
        }

        keyEnemy = Random.Range(0, enemies.Count);
    }

    // Update is called once per frame
    void Update()
    {
        #region Let Random Enemy Drop Key

        if (enemies[keyEnemy])
        {
            lastPos = enemies[keyEnemy].transform.position;
        }
        if (!enemies[keyEnemy] && !keyGenerated)
        {
            Instantiate(key, lastPos, Quaternion.identity, transform);
            keyGenerated = true;
        }

        #endregion
    }
}
