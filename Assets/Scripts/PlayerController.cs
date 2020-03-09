using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject[] HP;
    public GameObject Bullet;
    public Transform firePoint;
    public float fireRate;

    public AudioClip KeyGet; //done

    public AudioClip Run; //done

    public AudioClip playerLaser; //done

    public AudioClip Ouch; //Done
    
    public AudioClip WeaponGet; //In Progress, tentative to change

    public AudioClip HealthUp;

    public FloatingJoystick leftJS, rightJS;

    AudioSource playerSource;

    public static int LP;

    bool invincible;
    SpriteRenderer sr;
    Collider2D c;
    Rigidbody2D rb;

    //animation
    Animator anim;
    SpriteRenderer characterSprite;

    // Start is called before the first frame update
    void Start()
    {
        invincible = false;
        LP = 3;
        HP[0].SetActive(true);
        HP[1].SetActive(true);
        HP[2].SetActive(true);

        HP[0].GetComponent<Image>().color = new Color(HP[0].GetComponent<Image>().color.r, HP[0].GetComponent<Image>().color.g, HP[0].GetComponent<Image>().color.b, 1f);
        HP[1].GetComponent<Image>().color = new Color(HP[1].GetComponent<Image>().color.r, HP[1].GetComponent<Image>().color.g, HP[1].GetComponent<Image>().color.b, 1f);
        HP[2].GetComponent<Image>().color = new Color(HP[2].GetComponent<Image>().color.r, HP[2].GetComponent<Image>().color.g, HP[2].GetComponent<Image>().color.b, 1f);

        rb = GetComponent<Rigidbody2D>();
        c = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        InvokeRepeating("Shoot", 0, fireRate);

        playerSource = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();
        characterSprite = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Fire a bullet
    /// </summary>
    void Shoot()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
		//shoot in phone
        if (rightJS.Horizontal != 0 || rightJS.Vertical != 0)
		{
			Instantiate(Bullet, firePoint.position, transform.rotation);
            playerSource.PlayOneShot(playerLaser);
            playerLaser.pitch = Random.Range(0.1f, 0.9f);
		}
        #else
        //shoot in laptop
        if (Input.GetButton("Jump"))
        {
            Instantiate(Bullet, firePoint.position, transform.rotation);
            playerSource.PlayOneShot(playerLaser);
            playerSource.pitch = Random.Range(.85f, 1.15f);
        }
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.deltaTime > 0)
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            //Player Movement on phone
            float x = leftJS.Horizontal;
            float y = leftJS.Vertical;
            if (x != 0 || y != 0)
            {
                rb.velocity = new Vector2(Mathf.Sign(x)*Mathf.Clamp(Mathf.Abs(x * speed),1f,speed), Mathf.Sign(y)*Mathf.Clamp(Mathf.Abs(y * speed),1f,speed));
            }
            

            //Player Rotation on phone
            if (rightJS.Horizontal != 0 && rightJS.Vertical != 0)
            {
                transform.up = rightJS.Direction.normalized;
                playerSource.PlayOneShot(Run);
            }
            //animation
            if (y > 0)
            {
                anim.SetBool("isUp", true);
            }
            else
            {
                anim.SetBool("isUp", false);
            }

            if (y < 0)
            {
                anim.SetBool("isDown", true);
            }
            else
            {
                anim.SetBool("isDown", false);
            }

            if (Mathf.Abs(x) > 0)
            {
                anim.SetBool("isHorizontal", true);
            }
            else
            {
                anim.SetBool("isHorizontal", false);
            }

            if (x >= 0)
            {
                characterSprite.flipX = false;
            }
            else
            {
                characterSprite.flipX = true;
            }
            #else
            //Disable joysticks
            leftJS.gameObject.SetActive(false);
            rightJS.gameObject.SetActive(false);

            //Player Movement in Laptop
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(x * speed, y * speed);

            //Player Rotation in Laptop
            Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.up = (mouseScreenPosition - (Vector2)transform.position).normalized;

            //animation
            if (y > 0)
            {
                anim.SetBool("isUp", true);
            }
            else
            {
                anim.SetBool("isUp", false);
            }

            if (y < 0)
            {
                anim.SetBool("isDown", true);
            }
            else
            {
                anim.SetBool("isDown", false);
            }

            if (Mathf.Abs(x) > 0)
            {
                anim.SetBool("isHorizontal", true);
            }
            else
            {
                anim.SetBool("isHorizontal", false);
            }

            if (x >= 0)
            {
                characterSprite.flipX = false;
            }
            else
            {
                characterSprite.flipX = true;
            }
            #endif

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If lp<3 and on trigger enter health pick up
        if (LP < 3 && collision.CompareTag("HealthPickUp"))
        {
            playerSource.PlayOneShot(HealthUp);
            HP[LP].GetComponent<Image>().color = new Color(HP[LP].GetComponent<Image>().color.r, HP[LP].GetComponent<Image>().color.g, HP[LP].GetComponent<Image>().color.b, 1f);
            LP++;
            Destroy(collision.gameObject);
        }

        //If hit by enemy bullet, destroy bullet and reduce health
        if (collision.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
            if (!invincible)
            {
                LP -= 1;
                Hurt();
                playerSource.PlayOneShot(Ouch);
            }
            
        }
        //if hit by enemy, reduce health
        if (collision.CompareTag("Enemy"))
        {
            if (!invincible)
            {
                LP -= 1;
                Hurt();
                playerSource.PlayOneShot(Ouch);
                playerSource.volume = .9f;
            }
        }

        //if collect key, set bool to true
        if (collision.CompareTag("Key"))
        {
            //int n = System.Array.IndexOf(GameManage.rooms, collision.gameObject.transform.parent.gameObject);
            int n = GameManage.rooms.IndexOf(collision.gameObject.transform.parent.gameObject);
            GameManage.keyCollected[n] = true;
            playerSource.PlayOneShot(KeyGet);
            Destroy(collision.gameObject);
        }

        //for testing, opens game over canvas in GameManager script
        if (collision.gameObject.name == "Goal")
        {
            Destroy(collision.gameObject);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
	{
        if (collision.CompareTag("Enemy"))
        {
            if (!invincible)
            {
                LP -= 1;
                Hurt();
                playerSource.PlayOneShot(Ouch);
            }
        }
    }

    /// <summary>
    /// Reduce HP if Hurt
    /// </summary>
    void Hurt()
    {
        HP[LP].GetComponent<Image>().color = new Color(HP[LP].GetComponent<Image>().color.r, HP[LP].GetComponent<Image>().color.g, HP[LP].GetComponent<Image>().color.b, .3f);
        if (LP == 0)
        {
            Destroy(gameObject);
        }

        StopAllCoroutines();
        sr.color = new Color(255, 255, 255, 1);
        c.enabled = true;
        //StartCoroutine("NotCollide");
        StartCoroutine("Blink");
    }

    /*
    /// <summary>
    /// A Period of Invincible Time After Hurt
    /// </summary>
    /// <returns></returns>
    IEnumerator NotCollide()
    {
        c.enabled = false;
        yield return new WaitForSeconds(.5f);
        c.enabled = true;
        yield break;
    }
    */

    /// <summary>
    /// Blink Animation When Hurt
    /// </summary>
    /// <returns></returns>
    IEnumerator Blink()
    {
        invincible = true;
        for (int i = 0; i < 3; i++)
        {
            sr.color = new Color(255, 255, 255, 0);
            yield return new WaitForSeconds(.1f);
            sr.color = new Color(255, 255, 255, 1);
            yield return new WaitForSeconds(.1f);
        }
        invincible = false;
    }
}
