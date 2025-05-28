using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rob07Ctrl : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform [] bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 5;
    public float particleDalay = 0.5f;

    private int i = 0;

   
    Animator anim;
    CharacterController controller;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        anim.SetFloat("speedMultiplier", speed);

    }

    // Update is called once per frame
    void Update()
    {
       // anim.SetBool("shoot", false);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetAxis("Horizontal") > 0.1f)
            {
                anim.SetBool("turnLeft", true);
                anim.SetBool("turnRight", false);
            }
            if (Input.GetAxis("Horizontal") < -0.1f)
            {
                anim.SetBool("turnRight", true);
                anim.SetBool("turnLeft", false);
            }
        }
        else
        {
            anim.SetBool("turnLeft", false);
            anim.SetBool("turnRight", false);
        }

            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
            {
                anim.SetFloat("Turns", Input.GetAxis("Horizontal"));
            }
            else
            {
                anim.SetFloat("Turns", 0);
            }

            if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
            {
                anim.SetFloat("Speed", Input.GetAxis("Vertical"));
            }
            else
            {
                anim.SetFloat("Speed", 0);
            }

            if (Input.GetKeyDown("e"))
            {
                anim.SetBool("shoot", true);
               // StartCoroutine(StartDelay());
            }
            if (Input.GetKeyUp("e"))
            {
                anim.SetBool("shoot", false);
               // StartCoroutine(StartDelay());
            }

            if ((Input.GetKeyDown("p"))) //death
            {
                anim.SetBool("die", true);
                this.enabled = false;

            } 

            if ((Input.GetKeyDown("u"))) //look around
            {
                anim.SetBool("look1", true);
            } else { anim.SetBool("look1", false); }

            if ((Input.GetKeyDown("o"))) //take damage 1
            {
                anim.SetBool("hitLeft", true);
            } else { anim.SetBool("hitLeft", false); }

            if ((Input.GetKeyDown("i"))) //take damage 2
            {
                anim.SetBool("hitRight", true);
            } else { anim.SetBool("hitRight", false); }        
    }

    void Shoot ()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint[i].position, bulletSpawnPoint[i].rotation);
        bullet.GetComponent<Rigidbody>().linearVelocity = bulletSpawnPoint[i].forward * bulletSpeed;
        i++;
        if (i >= 1) i = 0;
    }

/*
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(particleDalay);
        var bullet1 = Instantiate(bulletPrefab, bulletSpawnPoint[0].position, bulletSpawnPoint[0].rotation);
        bullet1.GetComponent<Rigidbody>().velocity = bulletSpawnPoint[0].forward * bulletSpeed;
        var bullet2 = Instantiate(bulletPrefab, bulletSpawnPoint[1].position, bulletSpawnPoint[1].rotation);
        bullet2.GetComponent<Rigidbody>().velocity = bulletSpawnPoint[1].forward * bulletSpeed;

    }
*/
}

