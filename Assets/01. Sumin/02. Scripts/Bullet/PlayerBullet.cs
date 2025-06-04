using System.Collections;
using UnityEngine;

public class PlayerBullet : MonoBehaviour, IBullet
{
    private float _speed = 30f;
    private float _damage;
    private Vector3 _direction;

    private void OnEnable()
    {
        StartCoroutine(CoReturn());
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(!other.CompareTag("Player") && other.TryGetComponent<IDamageable>(out var damageable))
        //{
        //    damageable.TakeDamage(_damage);
        //    BulletPoolManager.Instance.ReturnObject(gameObject, EBulletType.PlayerBullet);
        //}
        if (other.gameObject.layer == LayerMask.NameToLayer("AimCube") || other.CompareTag(nameof(ETags.Player)))
            return;

        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(_damage);
            UIEventHandler.Instance.OnPlayerAttackHit?.Invoke();
        }
        Debug.Log(other.name);

        BulletPoolManager.Instance.ReturnObject(gameObject, EBulletType.PlayerBullet);
    }

    private void Move()
    {
        transform.LookAt(transform.position + _direction);
        transform.position += _direction * _speed * Time.deltaTime;
    }

    public void SetStats(float damage, Vector3 dir, float explodeRange = 0f)
    {
        _damage = damage;
        _direction = dir;
    }

    public void SetTarget(GameObject target)
    {
        //나중에 타겟팅 생기면
    }

    private IEnumerator CoReturn()
    {
        yield return new WaitForSeconds(5f);

        BulletPoolManager.Instance.ReturnObject(gameObject, EBulletType.PlayerBullet);
    }
}
