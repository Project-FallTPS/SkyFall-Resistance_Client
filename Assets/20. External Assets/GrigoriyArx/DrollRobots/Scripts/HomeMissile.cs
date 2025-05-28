using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMissile : MonoBehaviour
{
    public float lifeTime = 3f;
    public GameObject HitSplash;
    public float Speed = 3f;
    public float StartSpeed = 2f;
    public float RotationSpeed = 2f;
    public float StartRotationSpeed = 1f;
    public Transform Target;
    [SerializeField]
    public float RocketStartDelay = 1f;

    [SerializeField]
    GameObject fx;

    public CapsuleCollider RocketCollider;
    Rigidbody rb;

    private bool isLookingAtObject = true;
    [SerializeField]
    private float focusDistance = 5;
    private bool active = false;

    private float currentSpeed;
    private float currenRotationSpeeed;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currenRotationSpeeed = StartRotationSpeed;
        currentSpeed = StartSpeed;

    }
    private void Awake()
    {

    }

    private void Update()
    {
        if (active)
        {
            Vector3 targetDirection = Target.position - transform.position;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, currenRotationSpeeed * Time.deltaTime, 0.0F);

            transform.Translate(Vector3.forward * Time.deltaTime * currentSpeed, Space.Self);

            if (Vector3.Distance(transform.position, Target.position) < focusDistance)
            {
                isLookingAtObject = false;
            }

            if (isLookingAtObject)
            {
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }


    public void Launch()
    {
        active = true;
        StartCoroutine(FindTargetDelay());
        StartCoroutine(RocketColliderDelay());
        transform.parent = null;
        Destroy(gameObject, lifeTime);
        fx.SetActive(true);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal); // turn to Normal
            Vector3 pos = contact.point;

            if (HitSplash != null)
            {
                var hitVFX = Instantiate(HitSplash, pos, rot);
            }

            Destroy(gameObject);
            Debug.Log("Collision happened");
        }

    }

    IEnumerator FindTargetDelay()
    {
        currenRotationSpeeed = StartRotationSpeed;
        currentSpeed = StartSpeed;

        yield return new WaitForSeconds(RocketStartDelay);
        currenRotationSpeeed = RotationSpeed;
        currentSpeed = Speed;
    }

    IEnumerator RocketColliderDelay()
    {
        yield return new WaitForSeconds(RocketStartDelay);
        RocketCollider.enabled = true;        
    }

}
