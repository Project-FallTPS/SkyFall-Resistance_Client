using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10f;
    [SerializeField]
    private float _lifeTime = 5f;

    private float _damage;
    public float Damage { get => _damage; set => _damage = value; }
    private void OnEnable()
    {
        StartCoroutine(LifeCycle());
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(nameof(ETags.Player)))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_damage);
            }
            DamageablePoolManager.Instance.ReturnObject(gameObject, EDamageableType.EnemyBullet);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private IEnumerator LifeCycle()
    {
        yield return new WaitForSeconds(_lifeTime);
        DamageablePoolManager.Instance.ReturnObject(gameObject, EDamageableType.EnemyBullet);
    }
}
