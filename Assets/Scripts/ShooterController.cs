using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    public float nBullets;
    public float shootGap, breakTime;
    public GameObject bullet;

    bool fired;

    // Start is called before the first frame update
    void Start()
    {
        fired = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!fired)
        {

            StartCoroutine(FireBullets());
        }
    }

    IEnumerator FireBullets()
    {
        Debug.Log("fired");
        fired = true;
        for (int i = 0; i < nBullets; i++)
        {
            yield return new WaitForSeconds(shootGap);
            Instantiate(bullet, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(breakTime);
        fired = false;
    }
}
