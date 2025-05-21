using UnityEngine;

public class PlayerBullet : MonoBehaviour, IBullet
{
    private float _speed;
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

        Debug.Log(other.name);

        BulletPoolManager.Instance.ReturnObject(gameObject, EBulletType.PlayerBullet);
    }

    private void Move()
    {
        transform.LookAt(transform.position + _direction);
        transform.position += _direction * _speed * Time.deltaTime;
    }

    public void SetStats(float damage, float speed, Vector3 dir)
    {
        _damage = damage;
        _speed = speed;
        _direction = dir;
        _isDirectionSet = true;
    }

    public void SetTarget(GameObject target)
    {
        //나중에 타겟팅 생기면
    }
}
