using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMinigun : MonoBehaviour
{
    public float lifeTime = 10f;



    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

}
