using UnityEngine;

public class PlayerExplodeBullet : MonoBehaviour, IBullet
{
    private float _speed = 30f;
    private float _damage;
    private Vector3 _direction;
    private float _explodeRange;

    public void SetStats(float damage, Vector3 dir, float explodeRange = 0)
    {
        _damage = damage;
        _direction = dir;
        _explodeRange = explodeRange;
    }

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, _explodeRange, LayerMask.GetMask("Enemy"));
            if (hits.Length > 0)
            {
                //Instantiate(_bombEffectPrefab, transform.position, Quaternion.identity);
                PlayerEffectPoolManager.Instance.GetObject(EPlayerEffectType.BombEffect, transform.position, Quaternion.identity);
            }

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<IDamageable>(out var damage))
                {
                    damage.TakeDamage(_damage);
                    Debug.Log($"폭탄! {_damage}");
                }
            }
        }
    }

    private void Move()
    {
        transform.LookAt(transform.position + _direction);
        transform.position += _direction * _speed * Time.deltaTime;
    }
}