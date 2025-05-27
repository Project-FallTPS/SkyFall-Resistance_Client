using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : BulletBase
{

    private void OnEnable()
    {
        StartCoroutine(LifeCycle());
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected override void Move()
    {
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
    }
}
