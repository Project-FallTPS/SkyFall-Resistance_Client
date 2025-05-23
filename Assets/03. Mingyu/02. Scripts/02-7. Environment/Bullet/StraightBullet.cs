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

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(nameof(ETags.Player)))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
            }
            DamageablePoolManager.Instance.ReturnObject(gameObject, _damageableType);
        }
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
