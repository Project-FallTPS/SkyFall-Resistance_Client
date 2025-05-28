using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : BulletBase
{
    protected override void Update()
    {
        base.Update();
    }

    protected override void Move()
    {
        transform.Translate(Vector3.forward * (_speed * Time.deltaTime));
    }
}
