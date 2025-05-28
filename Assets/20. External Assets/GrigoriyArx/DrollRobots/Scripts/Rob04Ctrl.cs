using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rob04Ctrl : MonoBehaviour
{
    public float Speed = 1.0f;
    public float BattleSpeed = 2f;
    public Transform[] RocketSpawnPoints;
    public GameObject RocketPrefab;
    //public GameObject RocketBar;

    public float RocketSpeed = 5f;
    public float RocketRotationSpeed = 2f;
    public float StartRocketSpeed = 3f;
    public float RocketStartRotationSpeed = 0.5f;

    public float TargetingDalay = 1.5f;

    bool battleState = false;

    public Transform Target;
    public List<GameObject> spawnedMissiles = new List<GameObject>();

    int rocketIndex = 0;

    bool ableToAttack = false;

    Animator anim;
    CharacterController controller;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        anim.SetFloat("speedMultiplier", Speed);

        ReloadRockets();

        if (Target==null)
        {
            ableToAttack = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("shoot", false);

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

            if (Input.GetKeyDown("1"))
            {
                battleState = false;
                anim.SetBool("battle", false);
            }

            if (Input.GetKeyDown("2"))
            {
                battleState = true;
                anim.SetBool("battle", true);
                ableToAttack = true;
            }

            if (Target == null)
            {
                ableToAttack = false;
            }

            if ((Input.GetKeyDown("e")&&(battleState)&&(ableToAttack))) //Attack
            {
                anim.SetBool("shoot", true);

                spawnedMissiles[rocketIndex-1].GetComponent<HomeMissile>().Launch();
                spawnedMissiles.RemoveAt(rocketIndex - 1);
                rocketIndex--;
                Debug.Log("rokcetindex="+ rocketIndex);

                if (rocketIndex==0)
                {
                    anim.SetBool("battle", false);
                }
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
    }

    void ReloadDelay()
    {
        ableToAttack = false;
    }
    void ReloadDelayEnd()
    {
        ableToAttack = true;
    }

    void ReloadRockets ()
    {
        Debug.Log("rokcetindex=" + rocketIndex);
        while (rocketIndex < RocketSpawnPoints.Length)
        {
            //spawnedMissiles.Add( Instantiate (RocketPrefab, RocketSpawnPoints[rocketIndex].position, Quaternion.identity,RocketSpawnPoints[rocketIndex].transform) as GameObject);
            spawnedMissiles.Add(Instantiate(RocketPrefab, RocketSpawnPoints[rocketIndex].position, RocketSpawnPoints[rocketIndex].transform.rotation, RocketSpawnPoints[rocketIndex].transform) as GameObject);
            spawnedMissiles[rocketIndex].GetComponent<HomeMissile>().Target = Target;
            spawnedMissiles[rocketIndex].GetComponent<HomeMissile>().Speed = RocketSpeed;
            spawnedMissiles[rocketIndex].GetComponent<HomeMissile>().StartSpeed = StartRocketSpeed;
            spawnedMissiles[rocketIndex].GetComponent<HomeMissile>().RotationSpeed = RocketRotationSpeed;
            spawnedMissiles[rocketIndex].GetComponent<HomeMissile>().StartRotationSpeed = RocketStartRotationSpeed;
            spawnedMissiles[rocketIndex].GetComponent<HomeMissile>().RocketStartDelay = TargetingDalay;
            rocketIndex++;
        }
        // Debug.Log("rokcetindex=" + rocketIndex);
        if (battleState)
        {
            anim.SetBool("battle", true);
        }
        
    }
}

