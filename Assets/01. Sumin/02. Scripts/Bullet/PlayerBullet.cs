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
        Vector3 hitPoint = contact.point;       // �浹 ����
        Vector3 hitNormal = contact.normal;     // �浹 ���� ���� ����
        Debug.Log($"�浹 ����: {hitPoint}, ����: {hitNormal}");

        //hitpoint�� hitnormal �������� �ǰ� ����Ʈ ����
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
        //���߿� Ÿ���� �����
    }
}
