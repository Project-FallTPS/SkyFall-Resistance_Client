using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rob05Ctrl : MonoBehaviour
{
    public float Speed = 1.0f;
    public float BattleSpeed = 2f;
    public Transform[] BulletSpawnPoints;
    public GameObject Barrel_L, Barrel_R;
    public GameObject bulletPrefab;
    public GameObject bulletCasingPrefab;

  //    public AnimationClip BulletsBelt;
  //    private Animation beltAnim;

    public float bulletSpeed = 5;
    public float BarrelRotationSpeed = 2f;
    public float BarrelAccelaration = 5f;
    private float currentRotationSpeed = 0.1f;
    public float bulletDaley = 1f;

    bool ableToAttack = false;
    bool shooting = false;

    public GameObject BulletBelt_L, BulletBelt_R;



    Animator anim;
    CharacterController controller;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        anim.SetFloat("speedMultiplier", Speed);
       // beltAnim = GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {

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
        }

        if (Input.GetKeyDown("e")) //Attack
        {
            shooting = true;
            StartCoroutine(BarrelsAcceleretaions());
        }
        
        if (Input.GetKeyUp("e"))
        {
            shooting = false;
            StartCoroutine(BarrelsSlowDown());
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

        if ((Input.GetKeyDown("p"))) //death
        {
            anim.SetBool("die", true);
            this.enabled = false;
        }
    }

    IEnumerator BarrelsAcceleretaions()
    {          
            while(currentRotationSpeed<BarrelRotationSpeed)
            {
                currentRotationSpeed += BarrelAccelaration;
                if (!shooting) yield break;
                Barrel_L.transform.Rotate(0, currentRotationSpeed * Time.deltaTime, 0);
                Barrel_R.transform.Rotate(0, -currentRotationSpeed * Time.deltaTime, 0);
                yield return null;
            }
        ableToAttack = true;
        StartCoroutine(ShootMF());
        StartCoroutine(BulletsGeneration());


    }

    IEnumerator Shooting()
    {
        while (ableToAttack)
        Barrel_L.transform.Rotate(0, BarrelRotationSpeed * Time.deltaTime, 0);
        Barrel_R.transform.Rotate(0, -BarrelRotationSpeed * Time.deltaTime, 0);
        yield return null;
    }

    IEnumerator BarrelsSlowDown()
    {
        ableToAttack = false;
        anim.SetBool("shoot", false);
        while (currentRotationSpeed > 0)
        {
            currentRotationSpeed -= BarrelAccelaration*0.5f;
            if (shooting) yield break;
            Barrel_L.transform.Rotate(0, currentRotationSpeed * Time.deltaTime, 0);
            Barrel_R.transform.Rotate(0, -currentRotationSpeed * Time.deltaTime, 0);
            yield return null;
        }
    }

    IEnumerator BulletsGeneration()
    {
        while (ableToAttack)
        {
            var bullet = Instantiate(bulletPrefab, BulletSpawnPoints[0].position, BulletSpawnPoints[0].rotation);
            bullet.transform.parent = this.transform;
            var bulletCasing = Instantiate(bulletCasingPrefab, BulletSpawnPoints[2].position, BulletSpawnPoints[2].rotation);

            var bullet2 = Instantiate(bulletPrefab, BulletSpawnPoints[1].position, BulletSpawnPoints[1].rotation);
            bullet2.transform.parent = this.transform;
            var bulletCasing2 = Instantiate(bulletCasingPrefab, BulletSpawnPoints[3].position, BulletSpawnPoints[3].rotation);
            yield return new WaitForSeconds(bulletDaley);

        }
    }

    IEnumerator ShootMF()
    {
        //beltAnim.clip = BulletsBelt;
        // beltAnim.Play();

        anim.SetBool("shoot", true);
        while (ableToAttack)
        {

            yield return null;

            Barrel_L.transform.Rotate(0, BarrelRotationSpeed * Time.deltaTime, 0);
            Barrel_R.transform.Rotate(0, -BarrelRotationSpeed * Time.deltaTime, 0);

        }

        // beltAnim.Stop();

    }

}

