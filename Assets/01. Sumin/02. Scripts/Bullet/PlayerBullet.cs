using UnityEngine;

public class PlayerBullet : MonoBehaviour, IBullet
{
    private float _speed = 30f;
    private float _damage;
    private Vector3 _direction;
    private bool _isDirectionSet;

    private void OnEnable()
    {
        _isDirectionSet = false;
    }

    private void Update()
    {
        if(!_isDirectionSet)
        {
            Move();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(!other.CompareTag("Player") && other.TryGetComponent<IDamageable>(out var damageable))
        //{
        //    damageable.TakeDamage(_damage);
        //    BulletPoolManager.Instance.ReturnObject(gameObject, EBulletType.PlayerBullet);
        //}
        if (other.gameObject.layer == LayerMask.NameToLayer("AimCube"))
            return;

        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(_damage);
        }
        Debug.Log(other.name);

        BulletPoolManager.Instance.ReturnObject(gameObject, EBulletType.PlayerBullet);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("AimCube"))
            return;

        if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(_damage);
        }
        ContactPoint contact = collision.contacts[0];
        Vector3 hitPoint = contact.point;       // 충돌 지점
        Vector3 hitNormal = contact.normal;     // 충돌 면의 법선 방향
        Debug.Log($"충돌 지점: {hitPoint}, 법선: {hitNormal}");

        //hitpoint에 hitnormal 방향으로 피격 이펙트 생성
    }

    private void Move()
    {
        transform.LookAt(transform.position + _direction);
        transform.position += _direction * _speed * Time.deltaTime;
    }

    public void SetStats(float damage, Vector3 dir)
    {
        _damage = damage;
        _direction = dir;
        _isDirectionSet = true;
    }

    public void SetTarget(GameObject target)
    {
        //나중에 타겟팅 생기면
    }
}
